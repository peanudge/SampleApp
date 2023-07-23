using API.Contract.Responses.Item;

namespace API.Client.Resources;

public interface ICatalogItemResource
{
    Task<ItemResponse> Get(Guid id, CancellationToken cancellationToken = default);
}
