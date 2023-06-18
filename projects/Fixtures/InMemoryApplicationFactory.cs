using System.Data.Common;
using System.Diagnostics;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fixtures;

public class InMemoryApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>, IDisposable where TProgram : class
{

    private readonly DbConnection _connection;

    public InMemoryApplicationFactory()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection.Close();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // a way to override the dependency injection services defined in the Startup class of the API project.
        builder.UseEnvironment("Testing")
            .ConfigureTestServices(services =>
            {
                var option = new DbContextOptionsBuilder<CatalogContext>()
                    .UseSqlite(_connection)
                    .EnableSensitiveDataLogging() // TODO: Research about this method!
                    .LogTo(msg => Debug.WriteLine(msg), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                    .Options;

                services
                    .AddScoped<CatalogContext>(provider => new TestCatalogContext(option));

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopeServices = scope.ServiceProvider;

                var db = scopeServices.GetRequiredService<CatalogContext>();
                db.Database.EnsureCreated();
            });
    }
}
