using Domain.Models;
using Domain.Responses.Item;

namespace Domain.Mappers;

public class GenreMapper : IGenreMapper
{
    public GenreResponse Map(Genre genre)
    {
        return new GenreResponse
        {
            GenreId = genre.GenreId,
            GenreDescription = genre.GenreDescription
        };
    }
}