using Cart.API.Responses.Cart;

namespace Cart.API.Services
{
    public interface ICatalogService
    {
        Task<CartItemResponse> EnrichCartItem(CartItemResponse item, CancellationToken cancellationToken);
    }
}
