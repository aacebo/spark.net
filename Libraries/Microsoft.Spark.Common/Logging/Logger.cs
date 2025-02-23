namespace Microsoft.Spark.Common.Logging;

public partial interface ILogger
{
    public void Error(params object?[] args);
    public void Warn(params object?[] args);
    public void Info(params object?[] args);
    public void Debug(params object?[] args);
    public void Log(LogLevel level, params object?[] args);
    public ILogger Child(string name);
    public bool IsEnabled(LogLevel level);
    public ILogger SetLevel(LogLevel level);
}

public partial interface ILogger<T> : ILogger
{
}