using AutoMapper;
using Domain.Models;
using Domain.Requests.Item;
using Domain.Responses.Item;

namespace Domain.Mappers;

public class CatalogProfile : Profile
{
    public CatalogProfile()
    {
        CreateMap<ItemResponse, Item>().ReverseMap();
        CreateMap<GenreResponse, Genre>().ReverseMap();
        CreateMap<ArtistResponse, Artist>().ReverseMap();
        CreateMap<Price, PriceResponse>().ReverseMap();
        CreateMap<AddItemRequest, Item>().ReverseMap();
        CreateMap<EditItemRequest, Item>().ReverseMap();
    }
}