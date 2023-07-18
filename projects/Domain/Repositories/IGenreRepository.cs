using Domain.Models;

namespace Domain.Repositories;

public interface IGenreRepository : IRepository
{
    Task<IEnumerable<Genre>> GetAsync();
    Task<Genre?> GetAsync(Guid id);
    Genre Add(Genre item);
}
