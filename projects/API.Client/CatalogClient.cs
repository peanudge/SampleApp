using API.Client.Base;
using API.Client.Resources;

namespace API.Client;

public class CatalogClient : ICatalogClient
{
    public ICatalogItemResource Item { get; init; }
    public CatalogClient(HttpClient client)
    {
        if (client.BaseAddress is null) throw new ArgumentNullException(nameof(client.BaseAddress));

        Item = new CatalogItemResource(new BaseClient(client, client.BaseAddress.ToString()));
    }

}
