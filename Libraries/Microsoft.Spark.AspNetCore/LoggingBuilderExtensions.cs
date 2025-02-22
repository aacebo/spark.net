using System.Reflection;

using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder, string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        name ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark";
        return builder.AddProvider(new SparkLoggerProvider(new SparkLogger(name, level)));
    }

    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder, Common.Logging.ILogger logger)
    {
        return builder.AddProvider(new SparkLoggerProvider(new SparkLogger(logger)));
    }
}