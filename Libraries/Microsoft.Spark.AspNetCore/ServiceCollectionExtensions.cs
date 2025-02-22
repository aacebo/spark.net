using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpark(this IServiceCollection collection, Common.Logging.ILogger logger)
    {
        var log = new SparkLogger(logger);

        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(logger);

        return collection;
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        name ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark";

        var logger = new Common.Logging.ConsoleLogger(name, level);
        var log = new SparkLogger(logger);

        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton<Common.Logging.ILogger>(logger);

        return collection;
    }
}