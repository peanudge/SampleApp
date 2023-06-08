using System.Data.Common;
using Domain.Models;
using Intrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests
{
    public class ItemRepositoryTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<CatalogContext> _contextOptions;

        public ItemRepositoryTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlite(_connection)
                .Options;
        }

        public void Dispose()
        {
            _connection.Close();
        }

        [Fact]
        public async Task ShouldGetAllItem()
        {
            // Given 
            using (var context = new CatalogContext(_contextOptions))
            {
                var exist = context.Database.EnsureCreated();
                Assert.True(exist);
                context.Items.Add(GenerateSingleMockItem());
                context.Items.Add(GenerateSingleMockItem());
                context.SaveChanges();
            }

            // When
            using (var context = new CatalogContext(_contextOptions))
            {
                var repository = new ItemRepository(context);
                var result = await repository.GetAsync();

                // Then
                var items = Assert.IsType<List<Item>>(result);
                Assert.NotEmpty(items);
            }
        }

        [Fact]
        public async Task ShouldGetItemById()
        {
            var targetItem = GenerateSingleMockItem();

            // Given 
            using (var context = new CatalogContext(_contextOptions))
            {
                var exist = context.Database.EnsureCreated();
                Assert.True(exist);
                context.Items.Add(targetItem);
                context.SaveChanges();
            }

            using (var context = new CatalogContext(_contextOptions))
            {
                // When
                var repository = new ItemRepository(context);
                var result = await repository.GetAsync(targetItem.Id);

                // Then
                var item = Assert.IsType<Item>(result);
                Assert.NotNull(item);
            }
        }

        [Fact]
        public async Task ShouldGetNullByWrongId()
        {
            var targetItem = GenerateSingleMockItem();

            // Given 
            using (var context = new CatalogContext(_contextOptions))
            {
                var exist = context.Database.EnsureCreated();
                Assert.True(exist);
                context.Items.Add(targetItem);
                context.SaveChanges();
            }

            using (var context = new CatalogContext(_contextOptions))
            {
                // When
                var repository = new ItemRepository(context);
                var diffGuid = Guid.NewGuid();
                var result = await repository.GetAsync(diffGuid);

                // Then 
                Assert.Null(result);
            }
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
                ReleaseTime = DateTimeOffset.UtcNow,
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

        [Fact(Skip = "Not Implemented")]
        public void ShouldAddItem()
        {
            // Given 
            // When 
            // Then
            throw new NotImplementedException();
        }

        [Fact(Skip = "Not Implemented")]
        public void ShouldSoftDeleteItem()
        {
            // Given 
            // When 
            // Then
            throw new NotImplementedException();
        }
    }
}