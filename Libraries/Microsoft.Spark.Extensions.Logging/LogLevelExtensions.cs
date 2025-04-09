using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.Extensions.Logging;

public static class LogLevelExtensions
{
    public static Common.Logging.LogLevel ToSpark(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Information => Common.Logging.LogLevel.Info,
            LogLevel.Warning => Common.Logging.LogLevel.Warn,
            LogLevel.Error or LogLevel.Critical => Common.Logging.LogLevel.Error,
            _ => Common.Logging.LogLevel.Debug
        };
    }
}