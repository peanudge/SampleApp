using Domain.Models;
using Domain.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Intrastructure.Repositories
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
    }
}