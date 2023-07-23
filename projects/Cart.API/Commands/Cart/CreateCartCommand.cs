using Cart.API.Responses.Cart;
using MediatR;

namespace Cart.API.Commands.Cart;

public class CreateCartCommand : IRequest<CartExtendedResponse>
{
    public string[]? ItemIds { get; set; }
    public string? UserEmail { get; set; }
}
