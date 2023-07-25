using Cart.API.Configuration;

namespace Cart.API.Extensions;

public static class EventBusExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusSettings>(
                    configuration.GetSection(EventBusSettings.EventBus));
        return services;
    }
}
