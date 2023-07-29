using System.Collections.Concurrent;
using Cart.API.Entities;
using Cart.API.Responses.Cart;
using Cart.API.Services;
using Domain.Repositories;
using Moq;
using Newtonsoft.Json;

namespace Cart.API.Tests;

public class CartContextFactory
{
    public ICartRepository GetCartRepository()
    {
        var cartRepository = new Mock<ICartRepository>();
        var memoryCollection = GetMemoryCollection();

        cartRepository
            .Setup(x => x.GetCarts())
            .Returns(() =>
            {
                return memoryCollection.Keys.Select(x => x.ToString()).ToList();
            });

        cartRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
            {
                return Task.FromResult(JsonConvert.DeserializeObject<CartSession>(memoryCollection[id.ToString()]));
            });

        cartRepository
            .Setup(x => x.AddOrUpdateAsync(It.IsAny<CartSession>()))
            .Callback((CartSession x) =>
            {
                memoryCollection.AddOrUpdate(x.Id!, JsonConvert.SerializeObject(x), (key, oldValue) => JsonConvert.SerializeObject(x));
            })
            .ReturnsAsync((CartSession x) => JsonConvert.DeserializeObject<CartSession>(memoryCollection[x.Id!.ToString()]));

        return cartRepository.Object;
    }

    private static ConcurrentDictionary<string, string> GetMemoryCollection()
    {
        using (var reader = new StreamReader("./Data/cart.json"))
        {
            var json = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<CartSession[]>(json);
            if (data is null) throw new Exception("cart data is null");

            var memoryCollection = data.ToDictionary(x => x.Id!, JsonConvert.SerializeObject);
            var concurrentDictionary = new ConcurrentDictionary<string, string>(memoryCollection);
            return concurrentDictionary;
        }
    }

    public ICatalogService GetCatalogService()
    {
        var catalogService = new Mock<ICatalogService>();
        var itemResponse = GetItemResponse();
        catalogService.Setup(x => x.EnrichCartItem(It.IsAny<CartItemResponse>(), CancellationToken.None))
        .ReturnsAsync((CartItemResponse item, CancellationToken token) =>
        {
            var itemWithAllData = itemResponse[new Guid(item.CartItemId!)];
            item.Description = itemWithAllData.Description;
            item.Name = itemWithAllData.Name;
            item.Price = itemWithAllData.Price;
            item.ArtistName = itemWithAllData.ArtistName;
            item.GenreDescription = itemWithAllData.GenreDescription;
            item.PictureUri = itemWithAllData.PictureUri;
            return item;
        });

        return catalogService.Object;
    }

    private static Dictionary<Guid, CartItemResponse> GetItemResponse()
    {
        using (var reader = new StreamReader("./Data/items.json"))
        {
            var json = reader.ReadToEnd();
            var data = JsonConvert.DeserializeObject<CartItemResponse[]>(json);
            if (data is null) throw new Exception("item data is null");
            return data.ToDictionary(item => new Guid(item.CartItemId!), item => item);
        }
    }
}
