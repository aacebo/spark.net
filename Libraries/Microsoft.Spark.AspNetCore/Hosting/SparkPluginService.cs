using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps.Plugins;

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
        return Task.Run(() => _logger.LogDebug("Starting"));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("Start"));
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("Started"));
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("Stopping"));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("Stop"));
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => _logger.LogDebug("Stopped"));
    }
}