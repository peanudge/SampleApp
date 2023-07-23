using AutoMapper;
using Cart.API.Entities;
using Cart.API.Models;
using Cart.API.Responses.Cart;

namespace Cart.API.Mapper;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartItemResponse, CartItem>().ReverseMap();
        CreateMap<CartExtendedResponse, CartSession>().ReverseMap();
        CreateMap<CartUserResponse, CartUser>().ReverseMap();
    }
}
