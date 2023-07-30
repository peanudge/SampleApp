using System.Net;
using System.Text;
using Cart.API.Commands.Cart;
using Cart.API.Responses.Cart;
using Newtonsoft.Json;

namespace Cart.API.Tests;

public class CartControllerTests : IClassFixture<CartApplicationFactory<Program>>
{
    private readonly CartApplicationFactory<Program> _factory;

    public CartControllerTests(CartApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/v1/cart/9ced6bfa-9659-462e-aece-49fe50613e96")]
    public async Task SmokeTestOnGet(string url)
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("/api/v1/cart/9ced6bfa-9659-462e-aece-49fe50613e96")]
    public async Task GetShouldReturnRightData(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<CartExtendedResponse>(responseContent);

        Assert.Equal("9ced6bfa-9659-462e-aece-49fe50613e96", responseData!.Id);
        Assert.NotNull(responseData.Items?.Count);
        Assert.NotEmpty(responseData.User?.Email);
    }

    [Theory]
    [InlineData(new[] { "f5da5ce4-091e-492e-a70a-22b073d75a52", "be05537d-5e80-45c1-bd8c-aa21c0f1251e" }, "test@testdomain.com")]
    public async Task PostShouldCreateACart(string[] items, string email)
    {
        var client = _factory.CreateClient();

        var request = new CreateCartCommand
        {
            ItemIds = items,
            UserEmail = email
        };

        var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/api/v1/cart", httpContent);

        response.EnsureSuccessStatusCode();

        var responseHeader = response.Headers.Location;

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains("/api/v1/cart/", responseHeader?.ToString());
    }

    [Theory]
    [InlineData("9ced6bfa-9659-462e-aece-49fe50613e96", "f5da5ce4-091e-492e-a70a-22b073d75a52")]
    public async Task PutShouldIncreaseCartQuantity(string cartId, string cartItemId)
    {
        var client = _factory.CreateClient();

        var response = await client.PutAsync($"/api/v1/cart/{cartId}/items/{cartItemId}", new StringContent(string.Empty));

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("9ced6bfa-9659-462e-aece-49fe50613e96", "f5da5ce4-091e-492e-a70a-22b073d75a52")]
    public async Task DeleteShouldDecreaseCartQuantity(string cartId, string cartItemId)
    {
        var client = _factory.CreateClient();

        var response = await client.PutAsync($"/api/v1/cart/{cartId}/items/{cartItemId}", new StringContent(string.Empty));

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
