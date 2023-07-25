
using Cart.API.Entities;
using Cart.API.Events;
using Domain.Repositories;
using MediatR;

namespace Cart.API.Handlers.Cart.Events;

public class ItemSoldOutEventHandler : IRequestHandler<ItemSoldOutEvent, Unit>
{
    private readonly ICartRepository _cartRepository;

    public ItemSoldOutEventHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<Unit> Handle(ItemSoldOutEvent @event, CancellationToken cancellationToken)
    {
        var cartIds = _cartRepository.GetCarts().ToList();
        var tasks = cartIds.Select(async x =>
        {
            var cart = await _cartRepository.GetAsync(new Guid(x));

            if (cart is not null && @event.Id is not null)
            {
                await RemoveItemsInCart(@event.Id, cart);
            }
        });

        await Task.WhenAll(tasks);

        return Unit.Value;
    }
    private async Task RemoveItemsInCart(string itemToRemove, CartSession cartSession)
    {
        if (string.IsNullOrEmpty(itemToRemove)) return;

        var toDelete = cartSession.Items?.Where(x => x.CartItemId.ToString() == itemToRemove).ToList();

        if (toDelete == null || toDelete.Count == 0) return;

        foreach (var item in toDelete) cartSession.Items?.Remove(item);

        await _cartRepository.AddOrUpdateAsync(cartSession);
    }
}
