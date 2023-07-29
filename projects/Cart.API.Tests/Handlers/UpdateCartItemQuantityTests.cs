using AutoMapper;
using Cart.API.Commands.Cart;
using Cart.API.Handlers.Cart;
using Cart.API.Mapper;

namespace Cart.API.Tests.Hanlders;

public class UpdateCartItemQuantityTests : IClassFixture<CartContextFactory>
{
    private readonly CartContextFactory _cartContextFactory;

    public UpdateCartItemQuantityTests(CartContextFactory cartContextFactory)
    {
        _cartContextFactory = cartContextFactory;
    }

    [Fact]
    public async Task HandleShouldRemoveItemsWithQuantityZero()
    {
        var handler = new UpdateCartItemQuantity(
            _cartContextFactory.GetCartRepository(),
            new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
            _cartContextFactory.GetCatalogService()
        );

        var result = await handler.Handle(
            new UpdateCartItemQuantityCommand
            {
                CartId = new Guid("9ced6bfa-9659-462e-aece-49fe50613e96"),
                CartItemId = new Guid("f5da5ce4-091e-492e-a70a-22b073d75a52"),
                IsAddOperation = false
            },
            cancellationToken: CancellationToken.None
        );

        Assert.NotNull(result.Id);
        Assert.NotNull(result.Items);
        Assert.Single(result.Items);
    }

    [Fact]
    public async Task HandleShouldRetrieveItemWithIncreaseQuantity()
    {
        var handler = new UpdateCartItemQuantity(
            _cartContextFactory.GetCartRepository(),
            new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile<CartProfile>())),
            _cartContextFactory.GetCatalogService()
        );

        var result = await handler.Handle(
            new UpdateCartItemQuantityCommand
            {
                CartId = new Guid("9ced6bfa-9659-462e-aece-49fe50613e96"),
                CartItemId = new Guid("be05537d-5e80-45c1-bd8c-aa21c0f1251e"),
                IsAddOperation = true
            },
            cancellationToken: CancellationToken.None
        );

        Assert.NotNull(result.Id);
        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Items!.First(x => x.CartItemId == "be05537d-5e80-45c1-bd8c-aa21c0f1251e").Quantity);
        Assert.NotNull(result.User);
    }
}
