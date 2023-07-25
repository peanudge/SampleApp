
using Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Infrastructure.Exntensions;

public static class EventExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EventBusSettings>(
                    configuration.GetSection(EventBusSettings.EventBus));
        return services;
    }
}
