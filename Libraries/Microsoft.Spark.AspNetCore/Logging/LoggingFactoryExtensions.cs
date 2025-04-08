using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class LoggingFactoryExtensions
{
    public static ILoggerFactory AddSpark(this ILoggerFactory factory, string? name = null, Common.Logging.LoggingSettings? settings = null)
    {
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(name, settings)));
        return factory;
    }

    public static ILoggerFactory AddSpark(this ILoggerFactory factory, Common.Logging.ILogger logger)
    {
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(logger)));
        return factory;
    }
}