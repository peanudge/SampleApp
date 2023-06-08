using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQLServer");
        return services.AddDbContext<CatalogContext>(
            options => options.UseSqlServer(connectionString, serverOption =>
            {
                serverOption.MigrationsAssembly(typeof(Program).Assembly.FullName);
            }));
    }
}