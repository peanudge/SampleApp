using Domain.Models;
using Fixtures;
using Infrastructure.Respositories;

namespace Intrastructure.Tests.Repositories;

public class GenreRepositoryTests : IClassFixture<CatalogContextFactory>
{
    private readonly CatalogContextFactory _factory;

    public GenreRepositoryTests(CatalogContextFactory catalogContextFactory)
    {
        _factory = catalogContextFactory;
    }

    [Theory]
    [LoadData("genre")]
    public async Task ShouldReturnRecordById(Genre genre)
    {
        var respository = new GenreRepository(_factory.ContextInstance);
        var result = await respository.GetAsync(genre.GenreId);

        Assert.NotNull(result);
        Assert.Equal(result!.GenreId, genre.GenreId);
        Assert.Equal(result!.GenreDescription, genre.GenreDescription);
    }

    [Theory]
    [LoadData("genre")]
    public async Task ShouldAddNewItem(Genre genre)
    {
        var respository = new GenreRepository(_factory.ContextInstance);
        genre.GenreId = Guid.NewGuid();
        respository.Add(genre);
        var result = await respository.UnitOfWork.SaveEntitiesAsync();
        Assert.True(result);

        var addedData = await _factory.ContextInstance.Genres.FindAsync(genre.GenreId);
        Assert.NotNull(addedData);
    }
}
