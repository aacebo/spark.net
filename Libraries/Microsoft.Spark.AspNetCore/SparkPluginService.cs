using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public class SparkPluginService<TPlugin> : IHostedLifecycleService where TPlugin : IPlugin
{
    protected TPlugin _plugin;
    protected ILogger<SparkPluginService<TPlugin>> _logger;

    public SparkPluginService(TPlugin plugin, ILogger<SparkPluginService<TPlugin>> logger)
    {
        _plugin = plugin;
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

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("StartedAsync"));
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