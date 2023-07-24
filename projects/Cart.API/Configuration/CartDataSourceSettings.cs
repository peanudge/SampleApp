namespace Domain.Configuration;

public class CartDataSourceSettings
{
    public const string CartDataSource = "CartDataSource";

    public string? RedisConnectionString { get; set; }
}
