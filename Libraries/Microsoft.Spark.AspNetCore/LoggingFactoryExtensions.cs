using System.Reflection;

using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class LoggingFactoryExtensions
{
    public static ILoggerFactory AddSpark(this ILoggerFactory factory, string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        name ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark";
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(name, level)));
        return factory;
    }

    public static ILoggerFactory AddSpark(this ILoggerFactory factory, Common.Logging.ILogger logger)
    {
        factory.AddProvider(new SparkLoggerProvider(new SparkLogger(logger)));
        return factory;
    }
}