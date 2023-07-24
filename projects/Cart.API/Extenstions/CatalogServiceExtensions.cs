using API.Client;
using Cart.API.Extensions.Policies;
using Cart.API.Services;

namespace Cart.API.Extensions;

public static class CatalogServiceExtensions
{
    public static IServiceCollection AddCatalogService(this IServiceCollection services, Uri uri)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services
            .AddHttpClient<ICatalogClient, CatalogClient>(client => { client.BaseAddress = uri; })
            .SetHandlerLifetime(TimeSpan.FromMinutes(2))
            .AddPolicyHandler(CatalogServicePolicies.RetryPolicy())
            .AddPolicyHandler(CatalogServicePolicies.CircuitBreakerPolicy());

        return services;
    }
}
