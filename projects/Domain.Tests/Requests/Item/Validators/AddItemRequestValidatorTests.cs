
using Domain.Requests.Item;
using Domain.Requests.Item.Validators;
using FluentValidation.TestHelper;

namespace Domain.Tests.Requests.Item.Validators;

public class AddItemRequestValidatorTests
{
    private readonly AddItemRequestValidator _validator;

    public AddItemRequestValidatorTests()
    {
        _validator = new AddItemRequestValidator();
    }

    [Fact]
    public void ShouldHaveErrorWhenArtistIdIsNull()
    {
        // Given
        var addItemRequest = new AddItemRequest { Price = new Models.Price() };

        // When
        // Then
        _validator
            .TestValidate(addItemRequest)
            .ShouldHaveValidationErrorFor(x => x.ArtistId);
    }

    [Fact]
    public void ShouldHaveErrorWhenGenreIdIsNull()
    {
        // Given
        var addItemRequest = new AddItemRequest { Price = new Models.Price() };

        // When
        // Then
        _validator
            .TestValidate(addItemRequest)
            .ShouldHaveValidationErrorFor(x => x.GenreId);
    }
}