
using Domain.Requests.Artist;
using Domain.Requests.Genre;
using Domain.Requests.Item;
using Domain.Requests.Item.Validators;
using Domain.Responses.Item;
using Domain.Services;
using FluentValidation.TestHelper;
using Moq;

namespace Domain.Tests.Requests.Item.Validators;

public class AddItemRequestValidatorTests
{
    private readonly Mock<IArtistService> _artistServiceMock;
    private readonly Mock<IGenreService> _genreServiceMock;
    private readonly AddItemRequestValidator _validator;

    public AddItemRequestValidatorTests()
    {
        _artistServiceMock = new Mock<IArtistService>();
        _artistServiceMock
            .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>()))
            .ReturnsAsync(() => new ArtistResponse());

        _genreServiceMock = new Mock<IGenreService>();
        _genreServiceMock
            .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>()))
            .ReturnsAsync(() => new GenreResponse());

        _validator = new AddItemRequestValidator(_artistServiceMock.Object, _genreServiceMock.Object);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenArtistIdIsNull()
    {
        // When
        _artistServiceMock
            .Setup(x => x.GetArtistAsync(It.IsAny<GetArtistRequest>()))
            .ReturnsAsync(() => null);

        // Given
        var addItemRequest = new AddItemRequest { Price = new Models.Price(), ArtistId = Guid.NewGuid() };

        // Then
        var validationResult = await _validator.TestValidateAsync(addItemRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.ArtistId);
    }

    [Fact]
    public async Task ShouldHaveErrorWhenGenreIdIsNull()
    {
        // When
        _genreServiceMock
            .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>()))
            .ReturnsAsync(() => null);

        // Given
        var addItemRequest = new AddItemRequest { Price = new Models.Price(), GenreId = Guid.NewGuid() };

        // Then
        var validationResult = await _validator.TestValidateAsync(addItemRequest);
        validationResult.ShouldHaveValidationErrorFor(x => x.GenreId);
    }
}
