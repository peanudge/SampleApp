using System.Text;
using System.Text.Json;
using Domain.Configurations;
using Domain.Events;
using Domain.Mappers;
using Domain.Repositories;
using Domain.Requests.Item;
using Domain.Responses.Item;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Domain.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemMapper _itemMapper;
    private readonly ConnectionFactory _eventBusConnectionFactory;
    private readonly ILogger<ItemService> _logger;
    private readonly EventBusSettings _settings;

    public ItemService(
        IItemRepository itemRepository,
        IItemMapper itemMapper,
        IOptions<EventBusSettings> eventBusSettingsOption,
        ILogger<ItemService> logger)
    {
        var eventBusSettings = eventBusSettingsOption.Value;
        _itemRepository = itemRepository;
        _itemMapper = itemMapper;

        _eventBusConnectionFactory = new ConnectionFactory
        {
            HostName = eventBusSettings.HostName,
            UserName = eventBusSettings.User,
            Password = eventBusSettings.Password
        };

        _logger = logger;
        _settings = eventBusSettings;
    }

    public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
    {
        var item = _itemMapper.Map(request);
        var result = _itemRepository.Add(item);
        await _itemRepository.UnitOfWork.SaveChangesAsync();
        return _itemMapper.Map(result);
    }

    public async Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
    {
        var result = await _itemRepository.GetAsync(request.Id);
        if (result is null)
        {
            throw new ArgumentException($"Entity with {request.Id} is not present");
        }

        result.IsInactive = true;

        _itemRepository.Update(result);
        await _itemRepository.UnitOfWork.SaveChangesAsync();

        SendDeleteMessage(new ItemSoldOutEvent { Id = request.Id.ToString() });

        return _itemMapper.Map(result);
    }

    public async Task<ItemResponse> EditItemAsync(EditItemRequest request)
    {
        var existingRecord = _itemRepository.GetAsync(request.Id);
        if (existingRecord is null)
        {
            throw new ArgumentException($"Entity with {request.Id} is not present");
        }
        var entity = _itemMapper.Map(request);
        var result = _itemRepository.Update(entity);

        await _itemRepository.UnitOfWork.SaveChangesAsync();
        return _itemMapper.Map(result);
    }

    public async Task<ItemResponse?> GetItemAsync(GetItemRequest request)
    {
        if (request?.Id == null)
        {
            throw new ArgumentNullException();
        }

        var entity = await _itemRepository.GetAsync(request.Id);
        if (entity is null)
        {
            return null;
        }
        return _itemMapper.Map(entity);
    }

    public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
    {
        var result = await _itemRepository.GetAsync();
        return result.Select(x => _itemMapper.Map(x)!);
    }

    private void SendDeleteMessage(ItemSoldOutEvent message)
    {
        try
        {
            // TODO: Need to study
            var connection = _eventBusConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _settings.EventQueue, true, false);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.ConfirmSelect();
            channel.BasicPublish(exchange: "", routingKey: _settings.EventQueue, body: body);
            channel.WaitForConfirmsOrDie();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Unable to send message to event bus: {0}", e.Message);
        }
    }
}
