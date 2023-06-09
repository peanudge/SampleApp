using Domain.Models;
using Domain.Responses.Item;

namespace Domain.Mappers;

public class ArtistMapper : IArtistMapper
{
    public ArtistResponse Map(Artist artist)
    {
        return new ArtistResponse
        {
            ArtistId = artist.ArtistId,
            ArtistName = artist.ArtistName
        };
    }
}