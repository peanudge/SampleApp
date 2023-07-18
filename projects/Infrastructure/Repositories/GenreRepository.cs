using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories;

public class GenreRepository : IGenreRepository
{
    private readonly CatalogContext _catalogContext;

    public GenreRepository(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public IUnitOfWork UnitOfWork => _catalogContext;

    public Genre Add(Genre item)
    {
        return _catalogContext.Genres.Add(item).Entity;
    }

    public async Task<IEnumerable<Genre>> GetAsync()
    {
        return await _catalogContext.Genres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetAsync(Guid id)
    {
        return await _catalogContext.Genres
            .AsNoTracking()
            .Where(g => g.GenreId == id)
            .FirstOrDefaultAsync();
    }
}
