using Domain.Requests.Genre;
using Domain.Responses.Item;

namespace Domain.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreResponse>> GetGenresAsync();
    Task<GenreResponse?> GetGenreAsync(GetGenreRequest request);
    Task<IEnumerable<ItemResponse>> GetItemsByGenreIdAsync(GetGenreRequest request);
    Task<GenreResponse> AddGenreAsync(AddGenreRequest request);
}
