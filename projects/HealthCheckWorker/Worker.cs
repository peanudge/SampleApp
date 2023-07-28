using Microsoft.Extensions.Options;

namespace HealthCheckWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly HealthCheckSettings _settings;
    private HttpClient? _httpClient = null;

    public Worker(ILogger<Worker> logger,
        IOptions<HealthCheckSettings> healthCheckSettingsOptions
        )
    {
        _logger = logger;
        _settings = healthCheckSettingsOptions.Value;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _httpClient = new HttpClient();
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_httpClient is null)
        {
            throw new InvalidOperationException("The HTTP client is not initialized.");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _httpClient.GetAsync(_settings.Url, stoppingToken);
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"The web service is up. HTTP {result.StatusCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"The web service is down. {e.Message}");
            }

            await Task.Delay(_settings.IntervalMs, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _httpClient?.Dispose();
        return base.StopAsync(cancellationToken);
    }
}
