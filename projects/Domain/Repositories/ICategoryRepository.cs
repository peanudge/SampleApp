using Domain.DTO;
using Domain.Models;

namespace Domain.Repositories;

public interface ICategoryRepository
{
    public Task<List<Category>> GetRootCategories();
    public Task<Category?> GetCategoryById(long id);
    public Task<List<Category>> GetCategoriesWithSubCategoriesDTO();
    public Task<string?> GetCategoryPath(long categoryId);
}
