using Domain.Repositories;
using Intrastructure.Repositories;

namespace API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IItemRepository, ItemRepository>();
    }
}