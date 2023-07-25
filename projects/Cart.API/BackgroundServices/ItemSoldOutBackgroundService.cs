using Cart.API.Configuration;
using Cart.API.Events;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Cart.API.BackgroundServices;

public class ItemSoldOutBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EventBusSettings _eventBusSettings;
    private readonly ILogger<ItemSoldOutBackgroundService> _logger;
    private readonly IModel _channel;

    public ItemSoldOutBackgroundService(
        IOptions<EventBusSettings> eventBusSettingsOption,
        IServiceProvider serviceProvider,
        ILogger<ItemSoldOutBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _eventBusSettings = eventBusSettingsOption.Value;
        _logger = logger;
        var connectionFactory = new ConnectionFactory
        {
            HostName = _eventBusSettings.HostName,
            UserName = _eventBusSettings.User,
            Password = _eventBusSettings.Password
        };

        try
        {
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Unable to initialize the event bus: {message}", e.Message);
            throw;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (ch, ea) =>
        {
            var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            var @event = JsonConvert.DeserializeObject<ItemSoldOutEvent>(content);

            _logger.LogInformation("Consuming the following message from the event bus: {message}",
                    JsonConvert.SerializeObject(@event));

            if (@event is not null)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopedProvider = scope.ServiceProvider;
                        var mediator = scopedProvider.GetRequiredService<IMediator>();
                        await mediator.Send(@event, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            }
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        try
        {
            consumer.Model.QueueDeclare(
                queue: _eventBusSettings.EventQueue,
                durable: true,
                exclusive: false);
            _channel.BasicConsume(queue: _eventBusSettings.EventQueue, autoAck: false, consumer: consumer);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Unable to consume the event bus: {message}", e.Message);
        }

        return Task.CompletedTask;
    }
}
