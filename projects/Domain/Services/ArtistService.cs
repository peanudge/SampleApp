using Domain.Mappers;
using Domain.Models;
using Domain.Repositories;
using Domain.Requests.Artist;
using Domain.Responses.Item;

namespace Domain.Services;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IArtistMapper _artistMapper;
    private readonly IItemMapper _itemMapper;

    public ArtistService(IArtistRepository artistRepository,
                         IItemRepository itemRepository,
                         IArtistMapper artistMapper,
                         IItemMapper itemMapper)
    {
        _artistRepository = artistRepository;
        _itemRepository = itemRepository;
        _artistMapper = artistMapper;
        _itemMapper = itemMapper;
    }

    public async Task<ArtistResponse> AddArtistAsync(AddArtistRequest request)
    {
        var item = new Artist()
        {
            ArtistName = request.ArtistName
        };

        var result = _artistRepository.Add(item);

        await _artistRepository.UnitOfWork.SaveChangesAsync();

        return _artistMapper.Map(result);
    }

    public async Task<ArtistResponse?> GetArtistAsync(GetArtistRequest request)
    {
        var result = await _artistRepository.GetAsync(request.Id);
        return result == null ? null : _artistMapper.Map(result);
    }

    public async Task<IEnumerable<ArtistResponse>> GetArtistsAsync()
    {
        var result = await _artistRepository.GetAsync();
        return result.Select(_artistMapper.Map);
    }

    public async Task<IEnumerable<ItemResponse>> GetItemsByArtistIdAsync(GetArtistRequest request)
    {
        var result = await _itemRepository.GetItemByArtistIdAsync(request.Id);
        return result.Select(_itemMapper.Map);
    }
}
