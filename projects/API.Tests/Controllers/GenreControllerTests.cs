using System.Net;
using System.Text;
using API.ResponseModels;
using Domain.Models;
using Domain.Requests.Genre;
using Domain.Responses.Item;
using Fixtures;
using Newtonsoft.Json;

namespace API.Tests.Controllers;

public class GenreControllerTests : IClassFixture<InMemoryApplicationFactory<Program>>
{
    private readonly InMemoryApplicationFactory<Program> _factory;

    public GenreControllerTests(InMemoryApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/genre")]
    public async Task SmokeTestOnItems(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
    }


    [Theory]
    [InlineData("/api/v1/genre/?pageSize=1&pageIndex=0", 1, 0)]
    public async Task GetShouldReturnPaginatedData(string url, int pageSize, int pageIndex)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<PaginatedItemsResponseModel<GenreResponse>>(responseContent);

        Assert.Equal(responseEntity.PageIndex, pageIndex);
        Assert.Equal(responseEntity.PageSize, pageSize);
        Assert.Equal(responseEntity.Data.Count(), pageSize);
    }

    [Theory]
    [LoadData("genre")]
    public async Task GetByIdShouldReturnRightArtist(Genre request)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/v1/genre/{request.GenreId}");

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<GenreResponse>(responseContent);

        Assert.Equal(responseEntity.GenreId, request.GenreId);
    }

    [Fact]
    public async Task AddShouldCreateNewArtist()
    {
        var addGenreRequest = new AddGenreRequest()
        {
            GenreDescription = "Genre is a description"
        };

        var client = _factory.CreateClient();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(addGenreRequest),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("/api/v1/genre", httpContent);

        response.EnsureSuccessStatusCode();

        var responseLocationHeader = response.Headers.Location;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("/api/v1/genre/", responseLocationHeader!.ToString());
    }
}
