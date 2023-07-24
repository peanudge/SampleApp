using Cart.API.Configuration;
using RabbitMQ.Client;

namespace Cart.API.Extensions;

public static class EventsExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new EventBusSettings();
        configuration.Bind("EventBus", config);
        services.AddSingleton(config);

        var factory = new ConnectionFactory
        {
            HostName = config.HostName,
            UserName = config.User,
            Password = config.Password
        };

        services.AddSingleton(factory);
        return services;
    }
}
