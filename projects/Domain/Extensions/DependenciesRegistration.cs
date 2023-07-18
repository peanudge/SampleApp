using Domain.Mappers;
using Domain.Requests.Artist;
using Domain.Requests.Artist.Validators;
using Domain.Requests.Genre;
using Domain.Requests.Genre.Validators;
using Domain.Requests.Item;
using Domain.Requests.Item.Validators;
using Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Extensions;
public static class DependenciesRegistration
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        return services
            .AddSingleton<IArtistMapper, ArtistMapper>()
            .AddSingleton<IGenreMapper, GenreMapper>()
            .AddSingleton<IItemMapper, ItemMapper>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IItemService, ItemService>()
            .AddScoped<IGenreService, GenreService>()
            .AddScoped<IArtistService, ArtistService>();
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
            .AddScoped<IValidator<AddItemRequest>, AddItemRequestValidator>()
            .AddScoped<IValidator<EditItemRequest>, EditItemRequestValidator>()
            .AddScoped<IValidator<AddGenreRequest>, AddGenreRequestValidator>()
            .AddScoped<IValidator<AddArtistRequest>, AddArtistRequestValidator>();

    }
}
