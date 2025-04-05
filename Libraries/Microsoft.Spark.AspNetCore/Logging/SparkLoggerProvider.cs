using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

[ProviderAlias("SparkLogger")]
public class SparkLoggerProvider : ILoggerProvider
{
    protected SparkLogger? _logger;

    public SparkLoggerProvider(SparkLogger? logger = null)
    {
        _logger = logger;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger?.BeginScope(state);
    }

    public ILogger CreateLogger<T>()
    {
        var name = typeof(T).Name;
        return _logger?.Child(name) ?? new SparkLogger(name);
    }

    public ILogger CreateLogger(string name)
    {
        return _logger?.Child(name) ?? new SparkLogger(name);
    }

    public void Dispose()
    {
        _logger?.Dispose();
    }
}