using API.Client;
using Cart.API.Services;

namespace Cart.API;

public static class CatalogServiceExtensions
{
    public static IServiceCollection AddCatalogService(this IServiceCollection services, Uri uri)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddHttpClient<ICatalogClient, CatalogClient>(client => { client.BaseAddress = uri; });
        return services;
    }
}
