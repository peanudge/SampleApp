using Domain.Models;
using Fixtures;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class CategoryRespositoryTests : IClassFixture<CatalogContextFactory>
{
    private readonly CatalogContextFactory _factory;

    public CategoryRespositoryTests(CatalogContextFactory catalogContextFactory)
    {
        _factory = catalogContextFactory;
    }

    [Fact]
    public async Task ShouldReturnCorrectCategory()
    {
        var mockCategory = new Category
        {
            CategoryId = 1,
            Name = "Test Category 1",
            SubCategories = new List<Category> {
                new Category
                {
                    CategoryId = 2,
                    Name = "Test Category 2",
                    SubCategories = new List<Category> {
                        new Category
                        {
                            CategoryId = 3,
                            Name = "Test Category 3",
                            SubCategories = new List<Category>{}
                        }
                    }
                }}
        };

        _factory.ContextInstance.Categories.Add(mockCategory);
        await _factory.ContextInstance.SaveChangesAsync();
        _factory.ContextInstance.ChangeTracker.Clear();


        var respository = new CategoryRespository(_factory.ContextInstance);
        var categories = await respository.GetRootCategories();
        Assert.Single(categories);
        var rootCategory = categories.First();
        Assert.Single(rootCategory.SubCategories);
        // INFO: until only subcategory
        var subCategory = rootCategory.SubCategories.First();
        Assert.Null(subCategory.SubCategories);
    }


    [Fact]
    public async Task ShouldReturnAllSubCategories()
    {
        var mockCategory = new Category
        {
            CategoryId = 4,
            Name = "Test Category 1",
            SubCategories = new List<Category> {
                new Category
                {
                    CategoryId = 5,
                    Name = "Test Category 2",
                    SubCategories = new List<Category> {
                        new Category
                        {
                            CategoryId = 6,
                            Name = "Test Category 3",
                            SubCategories = new List<Category>{}
                        }
                    }
                }}
        };

        _factory.ContextInstance.Categories.Add(mockCategory);
        await _factory.ContextInstance.SaveChangesAsync();
        _factory.ContextInstance.ChangeTracker.Clear();


        var respository = new CategoryRespository(_factory.ContextInstance);
        var categories = await respository.GetCategoriesWithSubCategoriesDTO();
        Assert.Single(categories);
        var rootCategory = categories.First();
        Assert.Single(rootCategory.SubCategories);
        var subCategory = rootCategory.SubCategories.First();
        Assert.Single(subCategory.SubCategories);
    }
}
