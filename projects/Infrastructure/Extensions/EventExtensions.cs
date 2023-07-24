
using Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Exntensions;

public static class EventExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new EventBusSettings();
        // TODO: Change Option Pattern
        configuration.Bind("EventBus", config);
        services.AddSingleton(config);

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = config.HostName,
            UserName = config.User,
            Password = config.Password
        };

        services.AddSingleton(factory);
        return services;
    }
}
