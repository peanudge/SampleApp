using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        // How to handle more than context? multi IUnitOfWork? 
        private readonly CatalogContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ItemRepository(CatalogContext catalogContext)
        {
            _context = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task<IEnumerable<Item>> GetAsync()
        {
            return await _context
                .Items
                .TagWith("ItemRepository.GetAsync")
                .AsNoTracking()
                .Where(x => !x.IsInactive)
                .ToListAsync();
        }

        public async Task<Item?> GetAsync(Guid id)
        {
            return await _context
                .Items
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.Genre)
                .Include(x => x.Artist)
                .FirstOrDefaultAsync();
        }

        public Item Add(Item item)
        {
            return _context.Items.Add(item).Entity;
        }

        public Item Update(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public async Task<IEnumerable<Item>> GetItemByArtistIdAsync(Guid artistId)
        {
            var items = await _context.Items
                .AsNoTracking()
                .Where(x => !x.IsInactive)
                .Where(item => item.ArtistId == artistId)
                .Include(item => item.Artist)
                .Include(item => item.Genre)
                .ToListAsync();

            return items;
        }

        public async Task<IEnumerable<Item>> GetItemByGenreIdAsync(Guid genreId)
        {
            var items = await _context.Items
                .AsNoTracking()
                .Where(x => !x.IsInactive)
                .Where(item => item.GenreId == genreId)
                .Include(item => item.Artist)
                .Include(item => item.Genre)
                .ToListAsync();

            return items;
        }
    }
}
