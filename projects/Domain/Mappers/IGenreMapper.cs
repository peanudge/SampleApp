using Domain.Models;
using Domain.Responses.Item;
namespace Domain.Mappers;

public interface IGenreMapper
{
    GenreResponse Map(Genre genre);
}