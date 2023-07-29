using Cart.API.Events;
using Cart.API.Handlers.Cart.Events;

namespace Cart.API.Tests.Hanlders.Events;

public class ItemSoldOutEventHandlerTests : IClassFixture<CartContextFactory>
{
    private readonly CartContextFactory _cartContextFactory;

    public ItemSoldOutEventHandlerTests(CartContextFactory cartContextFactory)
    {
        _cartContextFactory = cartContextFactory;
    }

    [Fact]
    public async Task ShouldNotRemoveRecordsWhenSoldOutMessageContainsNotExistingItem()
    {
        var repository = _cartContextFactory.GetCartRepository();
        var itemSoldOutEventHandler = new ItemSoldOutEventHandler(repository);
        var found = false;

        await itemSoldOutEventHandler.Handle(new ItemSoldOutEvent
        {
            Id = Guid.NewGuid().ToString(),
        }, CancellationToken.None);

        var cartIds = repository.GetCarts();

        foreach (var cartId in cartIds)
        {
            var cart = await repository.GetAsync(new Guid(cartId));
            found = cart!.Items!.Any(i => i.CartItemId.ToString() == "be05537d-5e80-45c1-bd8c-aa21c0f1251e");
        }

        Assert.True(found);
    }

    [Fact]
    public async Task ShouldRemoveRecordsWhenSoldOutMessagesContainsExistingIds()
    {
        var itemSoldOutId = "be05537d-5e80-45c1-bd8c-aa21c0f1251e";
        var repository = _cartContextFactory.GetCartRepository();
        var itemSoldOutEventHandler = new ItemSoldOutEventHandler(repository);
        var found = false;

        await itemSoldOutEventHandler.Handle(new ItemSoldOutEvent
        {
            Id = itemSoldOutId,
        }, CancellationToken.None);

        var cartIds = repository.GetCarts();

        foreach (var cartId in cartIds)
        {
            var cart = await repository.GetAsync(new Guid(cartId));
            found = cart!.Items!.Any(i => i.CartItemId.ToString() == itemSoldOutId);
        }

        Assert.False(found);
    }
}
