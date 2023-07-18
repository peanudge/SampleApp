using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Respositories;

public class ArtistRepository : IArtistRepository
{
    private readonly CatalogContext _catalogContext;

    public IUnitOfWork UnitOfWork => _catalogContext;

    public ArtistRepository(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }


    public Artist Add(Artist item)
    {
        return _catalogContext.Artists.Add(item).Entity;
    }

    public async Task<IEnumerable<Artist>> GetAsync()
    {
        return await _catalogContext.Artists
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Artist?> GetAsync(Guid id)
    {
        return await _catalogContext.Artists
            .AsNoTracking()
            .Where(a => a.ArtistId == id)
            .FirstOrDefaultAsync();
    }
}
