using Domain.Mappers;
using Domain.Services;
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
            .AddScoped<IItemService, ItemService>();
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        return services
        .AddFluentValidationAutoValidation()
        .AddFluentValidationClientsideAdapters();
    }

}
