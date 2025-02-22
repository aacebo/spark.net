using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpark(this IServiceCollection collection, IAppOptions options)
    {
        var app = new App(options);
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        return collection.AddSingleton<IApp>(app);
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IAppBuilder builder)
    {
        var app = builder.Build();
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        return collection.AddSingleton(app);
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IApp app)
    {
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        return collection.AddSingleton(app);
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, IApp> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        return collection.AddSingleton(factory);
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, Task<IApp>> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        return collection.AddSingleton(provider => factory(provider).GetAwaiter().GetResult());
    }
}