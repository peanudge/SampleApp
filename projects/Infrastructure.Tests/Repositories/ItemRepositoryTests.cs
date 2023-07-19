using Domain.Models;
using Fixtures;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
    public class ItemRepositoryTests : IClassFixture<CatalogContextFactory>
    {
        private readonly TestCatalogContext _context;
        private readonly ItemRepository _repository;

        public ItemRepositoryTests(CatalogContextFactory catalogContextFactory)
        {
            _context = catalogContextFactory.ContextInstance;
            _repository = new ItemRepository(_context);
        }

        private Item GenerateSingleMockItem()
        {
            return new Item
            {
                Id = Guid.NewGuid(),
                Name = "Test Item 1",
                Description = "Test Item 1 Description",
                LabelName = "Test Item 1 LabelName",
                PictureUri = "Test Item 1 PictureUri",
                ReleaseDate = DateTimeOffset.UtcNow,
                Format = "Test Item 1 Format",
                AvailableStock = 1,
                Genre = new Genre
                {
                    GenreId = Guid.NewGuid(),
                    GenreDescription = "Test Genre 1 Description",
                },
                Artist = new Artist
                {
                    ArtistId = Guid.NewGuid(),
                    ArtistName = "Test Artist 1 Name",
                },
                Price = new Price
                {
                    Amount = 1,
                    Currency = "Test Price 1 Currency",
                },
            };
        }

        [Fact]
        public async Task ShouldGetAllItem()
        {
            // INFO: Why note use shared _context?
            // because this test want to prevent adding new item by other test.
            var contextOption = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "should_get_data")
                .EnableSensitiveDataLogging()
                .Options;

            using (var context = new CatalogContext(contextOption))
            {
                // Given   
                context.Items.Add(GenerateSingleMockItem());
                context.Items.Add(GenerateSingleMockItem());
                context.SaveChanges();

                var repository = new ItemRepository(context);
                // When 
                var result = await repository.GetAsync();

                // Then
                var items = Assert.IsType<List<Item>>(result);
                Assert.NotEmpty(items);
                Assert.Equal(2, items.Count);
            }
        }

        [Fact]
        public async Task ShouldGetItemById()
        {
            var targetItem = GenerateSingleMockItem();

            // Given   
            _context.Items.Add(targetItem);
            _context.SaveChanges();

            // When 
            var result = await _repository.GetAsync(targetItem.Id);

            // Then
            var item = Assert.IsType<Item>(result);
            Assert.NotNull(item);
        }

        [Fact]
        public async Task ShouldGetNullByWrongId()
        {
            var targetItem = GenerateSingleMockItem();

            // Given   
            _context.Items.Add(targetItem);
            _context.SaveChanges();

            // When
            var diffGuid = Guid.NewGuid();
            var result = await _repository.GetAsync(diffGuid);

            // Then 
            Assert.Null(result);

        }

        [Fact]
        public async Task ShouldAddItem()
        {
            var targetItem = GenerateSingleMockItem();

            // Given   

            // When 
            _repository.Add(targetItem);

            await _repository.UnitOfWork.SaveEntitiesAsync();

            var result = await _repository.GetAsync(targetItem.Id);

            // Then 
            var item = Assert.IsType<Item>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldUpdateItem()
        {
            var willUpdatedItem = GenerateSingleMockItem();

            // Given   
            _context.Items.Add(willUpdatedItem);
            _context.SaveChanges();

            // When 

            willUpdatedItem.Description = "Updated Description";
            _repository.Update(willUpdatedItem);

            await _repository.UnitOfWork.SaveEntitiesAsync();

            // Then 
            var result = _context.Items.FirstOrDefault(x => x.Id == willUpdatedItem.Id);
            var item = Assert.IsType<Item>(result);
            Assert.Equal("Updated Description", item.Description);
        }

        [Theory]
        [InlineData("f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task GetItemsShouldNotReturnInactiveRecords(string id)
        {
            var result = await _repository.GetAsync();
            Assert.DoesNotContain(result, (x => x.Id == new Guid(id)));
        }

        [Theory]
        [InlineData("f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task GetItemsByArtistShouldNotReturnInactiveRecords(string id)
        {
            var artistId = "3eb00b42-a9f0-4012-841d-70ebf3ab7474";

            var result = await _repository.GetItemByArtistIdAsync(new Guid(artistId));

            Assert.DoesNotContain(result, (x => x.Id == new Guid(id)));
        }

        [Theory]
        [InlineData("f5da5ce4-091e-492e-a70a-22b073d75a52")]
        public async Task GetItemsByGenreShouldNotReturnInactiveRecords(string id)
        {
            var genreId = "c04f05c0-f6ad-44d1-a400-3375bfb5dfd6";
            var result = await _repository.GetItemByGenreIdAsync(new Guid(genreId));
            Assert.DoesNotContain(result, (x => x.Id == new Guid(id)));
        }
    }
}
