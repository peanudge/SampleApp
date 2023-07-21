
using Fixtures;

namespace API.Tests.Middleware;

public class ResponseTimeMiddlewareTests : IClassFixture<InMemoryApplicationFactory<Program>>
{
    private readonly InMemoryApplicationFactory<Program> _factory;

    public ResponseTimeMiddlewareTests(InMemoryApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/items/?pageSize=1&pageIndex=0")]
    [InlineData("/api/v1/artist/?pageSize=1&pageIndex=0")]
    [InlineData("/api/v1/genre/?pageSize=1&pageIndex=0")]
    public async Task MiddlewareShouldSetTheCorrectResponseTimeCustomHeader(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        Assert.NotEmpty(response.Headers.GetValues("X-Response-Time-ms"));
    }
}
