using Cart.API.Responses.Cart;
using MediatR;

namespace Cart.API.Command.Cart;


public class GetCartCommand : IRequest<CartExtendedResponse?>
{
    public Guid Id { get; set; }
}
