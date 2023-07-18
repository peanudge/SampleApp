namespace Domain.Requests.Item.Validators;

using Domain.Services;
using FluentValidation;

public class AddItemRequestValidator : AbstractValidator<AddItemRequest>
{
    private readonly IArtistService _aritstService;
    private readonly IGenreService _genreService;

    public AddItemRequestValidator(IArtistService artistService,
                                   IGenreService genreService)
    {
        _aritstService = artistService;
        _genreService = genreService;

        RuleFor(x => x.ArtistId).NotEmpty()
            .MustAsync(ArtistExists)
            .WithMessage("Artist must exists.");

        RuleFor(x => x.GenreId).NotEmpty()
            .MustAsync(GenreExists)
            .WithMessage("Genre must exists.");

        RuleFor(x => x.GenreId).NotEmpty();
        RuleFor(x => x.ArtistId).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.ReleaseDate).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).Must(x => x?.Amount > 0);
        RuleFor(x => x.AvailableStock).Must(x => x > 0);
    }

    private async Task<bool> ArtistExists(Guid artistId, CancellationToken token)
    {
        if (string.IsNullOrEmpty(artistId.ToString()))
        {
            return false;
        }

        var artist = await _aritstService.GetArtistAsync(new Artist.GetArtistRequest()
        {
            Id = artistId
        });

        return artist != null;
    }

    private async Task<bool> GenreExists(Guid guid, CancellationToken token)
    {
        if (string.IsNullOrEmpty(guid.ToString()))
        {
            return false;
        }

        var genre = await _genreService.GetGenreAsync(new Genre.GetGenreRequest()
        {
            Id = guid
        });

        return genre != null;
    }
}
