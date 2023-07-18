using Domain.Repositories;
using Infrastructure.Repositories;

namespace API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IItemRepository, ItemRepository>()
            .AddScoped<IGenreRepository, GenreRepository>()
            .AddScoped<IArtistRepository, ArtistRepository>();
    }
}
