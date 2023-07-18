
using Domain.Requests.Artist;
using Domain.Responses.Item;

namespace Domain.Services;

public interface IArtistService
{
    Task<IEnumerable<ArtistResponse>> GetArtistsAsync();
    Task<ArtistResponse?> GetArtistAsync(GetArtistRequest request);
    Task<IEnumerable<ItemResponse>> GetItemsByArtistIdAsync(GetArtistRequest request);
    Task<ArtistResponse> AddArtistAsync(AddArtistRequest request);
}
