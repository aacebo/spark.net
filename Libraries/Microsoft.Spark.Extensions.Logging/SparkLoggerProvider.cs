using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.Extensions.Logging;

[ProviderAlias("Microsoft.Spark")]
public class SparkLoggerProvider : ILoggerProvider, IDisposable
{
    protected SparkLogger _logger;

    public SparkLoggerProvider(Common.Logging.ILogger logger)
    {
        _logger = new SparkLogger(logger);
    }

    public SparkLoggerProvider(SparkLogger logger)
    {
        _logger = logger;
    }

    public SparkLoggerProvider(Common.Logging.LoggingSettings settings)
    {
        _logger = new SparkLogger(new Common.Logging.ConsoleLogger(settings));
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }

    public ILogger CreateLogger<T>()
    {
        var name = typeof(T).Name;
        return _logger.Child(name);
    }

    public ILogger CreateLogger(string name)
    {
        return _logger.Child(name);
    }

    public void Dispose()
    {
        _logger.Dispose();
    }
}