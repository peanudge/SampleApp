using System.Text.Json;
using Cart.API.Entities;
using Domain.Options;
using Domain.Repositories;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly CartDataSourceSettings _settings;
    private readonly IDatabase _database;

    public CartRepository(IOptions<CartDataSourceSettings> options)
    {
        _settings = options.Value;

        if (_settings.RedisConnectionString is null)
        {
            throw new ArgumentNullException(nameof(_settings.RedisConnectionString));
        }

        var configuration = ConfigurationOptions.Parse(_settings.RedisConnectionString);

        try
        {
            _database = ConnectionMultiplexer.Connect(configuration).GetDatabase();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            throw;
        }
    }

    public async Task<CartSession?> AddOrUpdateAsync(CartSession item)
    {
        var created = await _database.StringSetAsync(item.Id, JsonSerializer.Serialize(item));
        if (!created) return null;

        return await GetAsync(new Guid(item.Id!));
    }

    public async Task<CartSession?> GetAsync(Guid id)
    {
        var data = await _database.StringGetAsync(id.ToString());
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CartSession>(data!);
    }

    public IEnumerable<string> GetCarts()
    {
        if (_settings.RedisConnectionString is null)
        {
            throw new ArgumentNullException(nameof(_settings.RedisConnectionString));
        }

        var keys = _database.Multiplexer.GetServer(_settings.RedisConnectionString).Keys();
        return keys?.Select(key => key.ToString()) ?? new List<string>();
    }

}
