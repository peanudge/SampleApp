using Domain.Configuration;

namespace Cart.API.Extensions;

public static class RedisExtension
{
    public static IServiceCollection AddRedisSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CartDataSourceSettings>(
            configuration.GetSection(CartDataSourceSettings.CartDataSource));
        //TODO: add Redis connection instance as singleton instance
        return services;
    }
}
