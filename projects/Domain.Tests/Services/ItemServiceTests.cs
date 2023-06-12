
using Domain.Mappers;
using Domain.Models;
using Domain.Requests.Item;
using Domain.Services;
using Fixtures;
using Intrastructure.Repositories;

namespace Domain.Tests.Services;

public class ItemServiceTests : IClassFixture<CatalogContextFactory>
{
    private readonly ItemRepository _itemRepository;
    private readonly IItemMapper _itemMapper;

    public ItemServiceTests(CatalogContextFactory catalogContextFactory)
    {
        _itemRepository = new ItemRepository(catalogContextFactory.ContextInstance);
        _itemMapper = catalogContextFactory.ItemMapper;
    }

    [Fact]
    public async Task ShouldReturnRightAllItemData()
    {
        // Given
        var itemService = new ItemService(_itemRepository, _itemMapper);
        // When
        var result = await itemService.GetItemsAsync();
        // Then
        Assert.NotEmpty(result);
    }

    [Theory]
    [InlineData("86bff4f7-05a7-46b6-ba73-d43e2c45840f")]
    public async Task ShouldReturnRightItemDataById(Guid itemId)
    {
        // Given
        var itemService = new ItemService(_itemRepository, _itemMapper);
        // When

        var result = await itemService.GetItemAsync(new GetItemRequest { Id = itemId });
        // Then 
        Assert.NotNull(result);
        Assert.Equal(itemId, result?.Id!);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenGetItemByNullId()
    {
        // Given
        var itemService = new ItemService(_itemRepository, _itemMapper);

        // When
        // Then
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await itemService.GetItemAsync(null!);
        });
    }

    [Fact]
    public async Task AddItemShouldAddRightEntity()
    {
        // Given
        var testItem = new AddItemRequest()
        {
            Name = "TestItem",
            GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
            ArtistId = new Guid("3eb00b42-a9f0-4012-841d-70ebf3ab7474"),
            Price = new Price { Amount = 13, Currency = "USD" },
            Description = "TestDescription",
            Format = "TestFormat",
            LabelName = "TestLabelName",
            PictureUri = "TestPictureUri",
            ReleaseDate = DateTimeOffset.Now,
            AvailableStock = 1
        };
        IItemService service = new ItemService(_itemRepository, _itemMapper);

        // When
        var result = await service.AddItemAsync(testItem);

        // Then
        Assert.Equal(testItem.Name, result.Name);
        Assert.Equal(testItem.GenreId, result.GenreId);
        Assert.Equal(testItem.ArtistId, result.ArtistId);
        Assert.Equal(testItem.Price.Amount, result.Price.Amount);
        Assert.Equal(testItem.Price.Currency, result.Price.Currency);
    }

    [Fact]
    public async Task EditItemShouldEditRightEntity()
    {
        // Given
        var testItem = new EditItemRequest()
        {
            Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
            Name = "TestItem",
            GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
            ArtistId = new Guid("3eb00b42-a9f0-4012-841d-70ebf3ab7474"),
            Price = new Price { Amount = 13, Currency = "USD" },
            Description = "TestDescription",
            Format = "TestFormat",
            LabelName = "TestLabelName",
            PictureUri = "TestPictureUri",
            ReleaseDate = DateTimeOffset.Now,
            AvailableStock = 1
        };

        // When
        IItemService service = new ItemService(_itemRepository, _itemMapper);
        var result = await service.EditItemAsync(testItem);

        // Then
        Assert.Equal(testItem.Name, result.Name);
        Assert.Equal(testItem.Description, result.Description);
        Assert.Equal(testItem.GenreId, result.GenreId);
        Assert.Equal(testItem.Price.Amount, result.Price.Amount);
        Assert.Equal(testItem.Price.Currency, result.Price.Currency);
    }
}