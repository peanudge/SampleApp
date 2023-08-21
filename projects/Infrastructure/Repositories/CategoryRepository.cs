using System.Linq.Expressions;
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

    private static Expression<Func<Category, Category>> GetCategoryProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Category, Category>> result = category => new Category()
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategory = category.ParentCategory,
            SubCategories = currentDepth == maxDepth ? new List<Category>() :
                category.SubCategories.AsQueryable()
                .Select(GetCategoryProjection(maxDepth, currentDepth))
                .ToList()
        };

        return result;
    }

    public async Task<List<Category>> GetCategoriesWithSubCategoriesDTO()
    {
        var query = _context.Categories
            .TagWith("GetCategoriesWithSubCategoriesDTO")
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Select(GetCategoryProjection(MAX_CATEGORY_DEPTH, 0));

        return await query.ToListAsync();
    }

    public async Task<string?> GetCategoryPath(long categoryId)
    {
        var allCategories = await GetCategoriesWithSubCategoriesDTO();

        foreach (var rootCategory in allCategories)
        {
            var result = FindCategoryPath(rootCategory, categoryId);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private string? FindCategoryPath(Category category, long targetCategoryId)
    {
        if (category.CategoryId == targetCategoryId)
        {
            return category.Name;
        }

        foreach (var subCategory in category.SubCategories)
        {
            var path = FindCategoryPath(subCategory, targetCategoryId);
            if (path != null)
            {
                return $"{category.Name} > {path}";
            }
        }

        return null;
    }
}
