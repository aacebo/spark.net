using Microsoft.Extensions.Logging;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder, string? name = null, LoggingSettings? settings = null)
    {
        return builder.AddProvider(new SparkLoggerProvider(new SparkLogger(name, settings)));
    }

    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder, Common.Logging.ILogger logger)
    {
        return builder.AddProvider(new SparkLoggerProvider(new SparkLogger(logger)));
    }
}