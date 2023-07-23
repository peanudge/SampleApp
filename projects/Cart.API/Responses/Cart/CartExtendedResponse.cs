using Cart.API.Entities;

namespace Cart.API.Responses.Cart;

public class CartExtendedResponse
{
    public string? Id { get; set; }
    public IList<CartItemResponse>? Items { get; set; }
    public CartUser? User { get; set; }
    public DateTimeOffset ValidityDate { get; set; }
}
