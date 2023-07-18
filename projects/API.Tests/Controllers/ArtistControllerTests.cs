using System.Net;
using System.Text;
using API.ResponseModels;
using Domain.Models;
using Domain.Requests.Artist;
using Domain.Responses.Item;
using Fixtures;
using Newtonsoft.Json;

namespace API.Tests.Controllers;

public class ArtistControllerTests : IClassFixture<InMemoryApplicationFactory<Program>>
{
    private readonly InMemoryApplicationFactory<Program> _factory;

    public ArtistControllerTests(InMemoryApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/artist")]
    public async Task SmokeTestOnItems(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("/api/artist/?pageSize=1&pageIndex=0", 1, 0)]
    public async Task GetShouldReturnPaginatedData(string url, int pageSize, int pageIndex)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<PaginatedItemsResponseModel<ArtistResponse>>(responseContent);

        Assert.Equal(responseEntity.PageIndex, pageIndex);
        Assert.Equal(responseEntity.PageSize, pageSize);
        Assert.Equal(responseEntity.Data.Count(), pageSize);
    }

    [Theory]
    [LoadData("artist")]
    public async Task GetByIdShouldReturnRightArtist(Artist request)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/api/artist/{request.ArtistId}");

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseEntity = JsonConvert.DeserializeObject<ArtistResponse>(responseContent);

        Assert.Equal(responseEntity.ArtistId, request.ArtistId);
    }

    [Fact]
    public async Task AddShouldCreateNewArtist()
    {
        var addArtistRequest = new AddArtistRequest()
        {
            ArtistName = "The Braze"
        };

        var client = _factory.CreateClient();
        var httpContent = new StringContent(
            JsonConvert.SerializeObject(addArtistRequest),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync("/api/artist", httpContent);

        response.EnsureSuccessStatusCode();

        var responseLocationHeader = response.Headers.Location;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("/api/artist/", responseLocationHeader!.ToString());
    }
}
