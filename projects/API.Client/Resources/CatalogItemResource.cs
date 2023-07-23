using API.Client.Base;
using API.Contract.Responses.Item;

namespace API.Client.Resources;

public class CatalogItemResource : ICatalogItemResource
{
    private readonly IBaseClient _client;

    public CatalogItemResource(IBaseClient client)
    {
        _client = client;
    }

    private Uri BuildUri(Guid id, string path = "")
    {
        return _client.BuildUri(string.Format("/api/v1/items/{0}", id, path));
    }

    public async Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var uri = BuildUri(id);
        return await _client.GetAsync<ItemResponse>(uri, cancellationToken);
    }
}
