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
        Assert.NotEmpty(categories);
        var rootCategory = categories.First();
        Assert.Single(rootCategory.SubCategories);
        var subCategory = rootCategory.SubCategories.First();
        Assert.Single(subCategory.SubCategories);
    }

    [Fact]
    public async Task ShouldCategoryPath()
    {
        var mockCategory = new Category
        {
            CategoryId = 7,
            Name = "1",
            SubCategories = new List<Category> {
                new Category
                {
                    CategoryId = 8,
                    Name = "2",
                    SubCategories = new List<Category> {
                        new Category
                        {
                            CategoryId = 9,
                            Name = "3",
                            SubCategories = new List<Category>{}
                        }
                    }
                }}
        };

        _factory.ContextInstance.Categories.Add(mockCategory);
        await _factory.ContextInstance.SaveChangesAsync();
        _factory.ContextInstance.ChangeTracker.Clear();


        var respository = new CategoryRespository(_factory.ContextInstance);
        var path = await respository.GetCategoryPath(9);
        Assert.NotNull(path);
        Assert.Equal("1 > 2 > 3", path);
    }
}
