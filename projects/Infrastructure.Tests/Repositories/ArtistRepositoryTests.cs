using Domain.Models;
using Fixtures;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class AritstRepositoryTest : IClassFixture<CatalogContextFactory>
{
    private readonly CatalogContextFactory _factory;
    public AritstRepositoryTest(CatalogContextFactory catalogContextFactory)
    {
        _factory = catalogContextFactory;
    }

    [Theory]
    [LoadData("artist")]
    public async Task ShouldReturnRecordById(Artist artist)
    {
        // Given 
        var repository = new ArtistRepository(_factory.ContextInstance);

        // When
        var result = await repository.GetAsync(artist.ArtistId);

        // Then
        Assert.NotNull(result);
        Assert.Equal(result!.ArtistId, artist.ArtistId);
        Assert.Equal(result.ArtistName, artist.ArtistName);
    }

    [Theory]
    [LoadData("artist")]
    public async Task ShouldAddNewItem(Artist artist)
    {
        var repoistory = new ArtistRepository(_factory.ContextInstance);
        artist.ArtistId = Guid.NewGuid();
        repoistory.Add(artist);
        var result = await repoistory.UnitOfWork.SaveEntitiesAsync();
        Assert.True(result);

        var addedData = await _factory.ContextInstance.Artists.FindAsync(artist.ArtistId);
        Assert.NotNull(addedData);
    }
}
