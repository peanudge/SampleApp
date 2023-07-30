using Cart.API.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cart.API.Tests;

public class CartApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    private readonly CartContextFactory _cartContextFactory;

    public CartApplicationFactory()
    {
        _cartContextFactory = new CartContextFactory();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing")
            .ConfigureTestServices(services =>
            {
                services.Replace(ServiceDescriptor.Scoped(serviceProvider =>
                {
                    return _cartContextFactory.GetCartRepository();
                }));

                services.Replace(ServiceDescriptor.Scoped(ServiceProvider =>
                {
                    return _cartContextFactory.GetCatalogService();
                }));

                var descriptor = services.SingleOrDefault(d => d.ImplementationType == typeof(ItemSoldOutBackgroundService));
                services.Remove(descriptor!);
            });
    }
}
