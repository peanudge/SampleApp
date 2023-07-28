using HealthCheckWorker;
using Microsoft.Extensions.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<HealthCheckSettings>(
            hostContext.Configuration.GetSection("HealthCheckSettings")
        );
        services.AddHostedService<Worker>(); // Under the hood, Singleton
    })
    .Build();


await host.RunAsync();
