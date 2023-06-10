using System.Data.Common;
using Domain.Mappers;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Fixtures;

public class CatalogContextFactory : IDisposable
{
    public readonly TestCatalogContext ContextInstance;
    public readonly IGenreMapper GenreMapper;
    public readonly IItemMapper ItemMapper;
    public readonly IArtistMapper ArtistMapper;

    private readonly DbConnection _connection;
    public CatalogContextFactory()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var contextOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging() // TODO: Research about this method!
            .Options;

        EnsureCreation(contextOptions);
        ContextInstance = new TestCatalogContext(contextOptions);
        GenreMapper = new GenreMapper();
        ArtistMapper = new ArtistMapper();
        ItemMapper = new ItemMapper(GenreMapper, ArtistMapper);
    }

    private void EnsureCreation(DbContextOptions<CatalogContext> contextOptions)
    {
        using (var context = new TestCatalogContext(contextOptions))
        {
            context.Database.EnsureCreated();
        };
    }

    public void Dispose()
    {
        _connection.Close();
    }
}