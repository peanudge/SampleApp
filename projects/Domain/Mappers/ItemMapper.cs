using Domain.Models;
using Domain.Requests.Item;
using Domain.Responses.Item;

namespace Domain.Mappers;

public class ItemMapper : IItemMapper
{
    private readonly IGenreMapper _genreMapper;
    private readonly IArtistMapper _artistMapper;

    public ItemMapper(IGenreMapper genreMapper, IArtistMapper artistMapper)
    {
        _genreMapper = genreMapper;
        _artistMapper = artistMapper;
    }

    public Item Map(AddItemRequest request)
    {
        var item = new Item
        {
            Name = request.Name,
            Description = request.Description,
            LabelName = request.LabelName,
            PictureUri = request.PictureUri,
            ReleaseDate = request.ReleaseDate,
            Format = request.Format,
            AvailableStock = request.AvailableStock,
            GenreId = request.GenreId,
            ArtistId = request.ArtistId,
        };

        if (request.Price != null)
        {
            item.Price = new Price
            {
                Currency = request.Price.Currency,
                Amount = request.Price.Amount
            };
        }
        return item;
    }

    public Item Map(EditItemRequest request)
    {
        var item = new Item
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            LabelName = request.LabelName,
            PictureUri = request.PictureUri,
            ReleaseDate = request.ReleaseDate,
            Format = request.Format,
            AvailableStock = request.AvailableStock,
            GenreId = request.GenreId,
            ArtistId = request.ArtistId,
        };

        if (request.Price != null)
        {
            item.Price = new Price
            {
                Currency = request.Price.Currency,
                Amount = request.Price.Amount
            };
        }
        return item;
    }

    public ItemResponse Map(Item item)
    {
        var response = new ItemResponse()
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            LabelName = item.LabelName,
            PictureUri = item.PictureUri,
            Price = new PriceResponse
            {
                Currency = item.Price.Currency,
                Amount = item.Price.Amount
            },
            ReleaseDate = item.ReleaseDate,
            Format = item.Format,
            AvailableStock = item.AvailableStock,
            GenreId = item.GenreId,
            Genre = item.Genre is not null ? _genreMapper.Map(item.Genre) : null,
            ArtistId = item.ArtistId,
            Artist = item.Artist is not null ? _artistMapper.Map(item.Artist) : null
        };
        return response;
    }
}