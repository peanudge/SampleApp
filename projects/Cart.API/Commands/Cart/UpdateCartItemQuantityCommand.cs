using Cart.API.Responses.Cart;
using MediatR;

namespace Cart.API.Commands.Cart
{
    public class UpdateCartItemQuantityCommand : IRequest<CartExtendedResponse>
    {
        public Guid CartId { get; set; }
        public Guid CartItemId { get; set; }
        public bool IsAddOperation { get; set; }
    }
}
