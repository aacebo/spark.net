using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public class SparkService : IHostedLifecycleService
{
    protected IApp _app;
    protected ILogger<SparkService> _logger;

    public SparkService(IApp app, ILogger<SparkService> logger)
    {
        _app = app;
        _logger = logger;
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StartingAsync"));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StartAsync"));
    }

    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        await _app.Start();
        _logger.LogDebug("StartedAsync");
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StoppingAsync"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StopAsync"));
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StoppedAsync"));
    }
}