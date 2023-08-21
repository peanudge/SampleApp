using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Domain.DTO;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryRespository : ICategoryRepository
{
    private const int MAX_CATEGORY_DEPTH = 4;

    private readonly CatalogContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public CategoryRespository(CatalogContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetCategoryById(long id)
    {
        return await _context
            .Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<List<Category>> GetRootCategories()
    {
        var allCategories = await _context
            .Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .ToListAsync();

        var categories = allCategories.Where(c => c.ParentCategoryId == null).ToList();

        return categories;
    }

    private static Expression<Func<Category, CategoryDTO>> GetCategoryProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Category, CategoryDTO>> result = category => new CategoryDTO()
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            SubCategories = currentDepth == maxDepth ? new List<CategoryDTO>() :
                category.SubCategories.AsQueryable()
                .Select(GetCategoryProjection(maxDepth, currentDepth))
                .ToList()
        };

        return result;
    }

    public async Task<List<CategoryDTO>> GetCategoriesWithSubCategoriesDTO()
    {
        var query = _context.Categories
            .TagWith("GetCategoriesWithSubCategoriesDTO")
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Select(GetCategoryProjection(MAX_CATEGORY_DEPTH, 0));

        return await query.ToListAsync();
    }
}
