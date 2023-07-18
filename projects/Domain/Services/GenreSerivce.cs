using Domain.Mappers;
using Domain.Repositories;
using Domain.Requests.Genre;
using Domain.Responses.Item;

namespace Domain.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IItemMapper _itemMapper;
        private readonly IGenreMapper _genreMapper;

        public GenreService(IGenreRepository genreRepository,
                            IItemRepository itemRepository,
                            IItemMapper itemMapper,
                            IGenreMapper genreMapper)
        {
            _genreRepository = genreRepository;
            _itemRepository = itemRepository;
            _itemMapper = itemMapper;
            _genreMapper = genreMapper;
        }

        public async Task<GenreResponse> AddGenreAsync(AddGenreRequest request)
        {
            var result = _genreRepository.Add(new Models.Genre
            {
                GenreDescription = request.GenreDescription
            });

            await _genreRepository.UnitOfWork.SaveChangesAsync();

            return _genreMapper.Map(result);
        }

        public async Task<GenreResponse?> GetGenreAsync(GetGenreRequest request)
        {
            var genre = await _genreRepository.GetAsync(request.Id);
            return genre == null ? null : _genreMapper.Map(genre);
        }

        public async Task<IEnumerable<GenreResponse>> GetGenresAsync()
        {
            var genres = await _genreRepository.GetAsync();
            return genres.Select(_genreMapper.Map);
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsByGenreIdAsync(GetGenreRequest request)
        {
            var items = await _itemRepository.GetItemByGenreIdAsync(request.Id);
            return items.Select(_itemMapper.Map);
        }
    }
}
