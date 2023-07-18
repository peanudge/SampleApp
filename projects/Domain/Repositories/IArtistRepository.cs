using Domain.Models;

namespace Domain.Repositories
{
    public interface IArtistRepository : IRepository
    {
        Task<IEnumerable<Artist>> GetAsync();
        Task<Artist?> GetAsync(Guid id);
        Artist Add(Artist item);
    }
}
