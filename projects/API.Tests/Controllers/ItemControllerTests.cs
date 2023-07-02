using System.Net;
using System.Text;
using API.ResponseModels;
using Domain.Models;
using Domain.Requests.Item;
using Domain.Responses.Item;
using Fixtures;
using Newtonsoft.Json;

namespace API.Tests.Controllers;

public class ItemControllerTests : IClassFixture<InMemoryApplicationFactory<Program>>
{
    private readonly InMemoryApplicationFactory<Program> _factory;

    public ItemControllerTests(InMemoryApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/items/")]
    public async Task GetShouldReturnSuccess(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [LoadData("item")]
    public async Task GetByIdShouldReturnSuccess(Item request)
    {
        // Given
        var client = _factory.CreateClient();

        // When
        var response = await client.GetAsync($"/api/v1/items/{request.Id}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<Item>(responseContent);

        // Then
        Assert.NotNull(responseEntity);
    }

    [Theory]
    [LoadData("item")]
    public async Task AddShouldCreateNewRecord(AddItemRequest request)
    {
        var client = _factory.CreateClient();

        var httpContent = new StringContent(
            JsonConvert.SerializeObject(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("/api/v1/items", httpContent);

        response.EnsureSuccessStatusCode();

        Assert.NotNull(response.Headers.Location);
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0#non-nullable-reference-types-and-required-attribute
    /// </summary> 
    [Fact]
    public async Task AddWithoutFormatShouldReturnBadRequest()
    {
        var request = new AddItemRequest
        {
            Name = "Test album",
            Description = "Description",
            LabelName = "Label name",
            Price = new Price { Amount = 13, Currency = "EUR" },
            PictureUri = "https://mycdn.com/pictures/32423423",
            ReleaseDate = DateTimeOffset.Now,
            AvailableStock = 5,
            ArtistId = Guid.Parse("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
            GenreId = Guid.Parse("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
            // INFO: Format is required parameter
            // Format = "Vinyl",
        };

        var client = _factory.CreateClient();

        var httpContent = new StringContent(
            JsonConvert.SerializeObject(request),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("/api/v1/items", httpContent);
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateShouldModifyExistingItem()
    {
        // Given 
        var request = new EditItemRequest()
        {
            Id = Guid.Parse("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
            Name = "Test album",
            Description = "Description",
            LabelName = "Label name",
            Price = new Price { Amount = 13, Currency = "EUR" },
            PictureUri = "https://mycdn.com/pictures/32423423",
            ReleaseDate = DateTimeOffset.Now,
            AvailableStock = 5,
            ArtistId = Guid.Parse("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
            GenreId = Guid.Parse("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
            Format = "Vinyl",
        };

        var client = _factory.CreateClient();

        var httpContent = new StringContent(
            JsonConvert.SerializeObject(request),
            Encoding.UTF8,
            "application/json");

        // When 
        var response = await client.PutAsync($"/api/v1/items/{request.Id}", httpContent);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<ItemResponse>(responseContent);

        Assert.Equal(request.Name, responseEntity.Name);
        Assert.Equal(request.Description, responseEntity.Description);
        Assert.Equal(request.GenreId, responseEntity.GenreId);
        Assert.Equal(request.ArtistId, responseEntity.ArtistId);
    }


    [Theory]
    [InlineData("/api/v1/items/?pageSize=1&pageIndex=0", 1, 0)]
    [InlineData("/api/v1/items/?pageSize=2&pageIndex=0", 2, 0)]
    [InlineData("/api/v1/items/?pageSize=1&pageIndex=1", 1, 1)]
    public async Task GetShouldReturnPaginatedData(string url, int pageSize, int pageIndex)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<PaginatedItemsResponseModel<ItemResponse>>(responseContent);

        Assert.Equal(pageIndex, responseEntity.PageIndex);
        Assert.Equal(pageSize, responseEntity.PageSize);
        Assert.Equal(pageSize, responseEntity.Data.Count());
    }
}
