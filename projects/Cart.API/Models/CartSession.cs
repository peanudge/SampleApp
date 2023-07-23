using Cart.API.Models;

namespace Cart.API.Entities;

public class CartSession
{
    public string? Id { get; set; }
    public IList<CartItem>? Items { get; set; }
    public CartUser? User { get; set; }
    public DateTimeOffset ValidityDate { get; set; }

}
