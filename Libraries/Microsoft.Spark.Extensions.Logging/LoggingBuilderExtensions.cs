using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Microsoft.Spark.Extensions.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder)
    {
        builder.AddConfiguration();
        builder.Services.AddSingleton<Common.Logging.ILogger, Common.Logging.ConsoleLogger>();
        builder.Services.AddSingleton<ILogger, SparkLogger>();
        builder.Services.AddSingleton<ILoggerProvider, SparkLoggerProvider>();
        LoggerProviderOptions.RegisterProviderOptions<Common.Logging.LoggingSettings, SparkLoggerProvider>(builder.Services);
        return builder;
    }

    public static ILoggingBuilder AddSpark(this ILoggingBuilder builder, Common.Logging.ILogger logger)
    {
        builder.AddConfiguration();
        builder.Services.AddSingleton(logger);
        builder.Services.AddSingleton<ILogger, SparkLogger>();
        builder.Services.AddSingleton<ILoggerProvider, SparkLoggerProvider>();
        LoggerProviderOptions.RegisterProviderOptions<Common.Logging.LoggingSettings, SparkLoggerProvider>(builder.Services);
        return builder;
    }
}