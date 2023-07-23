using Cart.API.Entities;

namespace Domain.Repositories;

public interface ICartRepository
{
    IEnumerable<string> GetCarts();
    Task<CartSession?> GetAsync(Guid id);
    Task<CartSession?> AddOrUpdateAsync(CartSession item);
}
