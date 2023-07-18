using Domain.Models;

namespace Domain.Repositories
{
    public interface IItemRepository : IRepository
    {
        Task<IEnumerable<Item>> GetAsync();
        Task<Item?> GetAsync(Guid id);
        Item Add(Item item);
        Item Update(Item item);

        Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid id);
        Task<IEnumerable<Item>> GetItemByGenreIdAsync(Guid id);
    }
}
