using System.Globalization;
using API.Client;
using API.Contract.Responses.Item;
using Cart.API.Responses.Cart;

namespace Cart.API.Services;

public class CatalogService : ICatalogService
{
    private readonly ICatalogClient _catalogClient;

    public CatalogService(ICatalogClient catalogClient)
    {
        _catalogClient = catalogClient;
    }

    public async Task<CartItemResponse> EnrichCartItem(CartItemResponse item, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _catalogClient.Item.Get(new Guid(item.CartItemId!), cancellationToken);
            return Map(item, result);
        }
        catch (Exception)
        {
            return item;
        }
    }

    private static CartItemResponse Map(CartItemResponse item, ItemResponse result)
    {
        item.Description = result.Description;
        item.Name = result.Name;
        item.LabelName = result.LabelName;
        item.Price = result.Price.Amount.ToString(CultureInfo.InvariantCulture);
        item.ArtistName = result.Artist?.ArtistName ?? "Unknown";
        item.PictureUri = result.PictureUri;
        return item;
    }
}
