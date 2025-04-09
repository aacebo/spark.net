using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.Extensions.Logging;

public class SparkLogger : ILogger, IDisposable
{
    public Common.Logging.ILogger Logger => _logger;

    protected Common.Logging.ILogger _logger;

    public SparkLogger(Common.Logging.LoggingSettings settings)
    {
        _logger = new Common.Logging.ConsoleLogger(settings);
    }

    public SparkLogger(Common.Logging.ILogger logger)
    {
        _logger = logger;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel level)
    {
        return _logger.IsEnabled(level.ToSpark());
    }

    public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(level.ToSpark(), formatter(state, exception));
    }

    public void Dispose()
    {
        // do nothing
    }

    public ILogger Create(string name)
    {
        return new SparkLogger(_logger.Create(name));
    }

    public ILogger Child(string name)
    {
        return new SparkLogger(_logger.Child(name));
    }

    public ILogger Peer(string name)
    {
        return new SparkLogger(_logger.Peer(name));
    }
}