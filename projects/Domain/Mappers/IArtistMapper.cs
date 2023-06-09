using Domain.Models;
using Domain.Responses.Item;
namespace Domain.Mappers;

public interface IArtistMapper
{
    ArtistResponse Map(Artist genre);
}