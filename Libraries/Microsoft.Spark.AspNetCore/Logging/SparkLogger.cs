using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public class SparkLogger : ILogger, IDisposable
{
    public Common.Logging.ILogger Logger { get => _logger; }
    protected Common.Logging.ILogger _logger;

    public SparkLogger(string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        _logger = new Common.Logging.ConsoleLogger(name, level);
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

    public ILogger Child(string name)
    {
        return new SparkLogger(_logger.Child(name));
    }
}