using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;

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
        collection.AddSingleton<IApp>(app);
        collection.AddHostedService<SparkService>();
        return collection.AddSparkPlugin(new AspNetCorePlugin());
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IAppBuilder builder)
    {
        var app = builder.Build();
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        return collection.AddSparkPlugin(new AspNetCorePlugin());
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IApp app)
    {
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        return collection.AddSparkPlugin(new AspNetCorePlugin());
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, IApp> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(factory);
        collection.AddHostedService<SparkService>();
        return collection.AddSparkPlugin(new AspNetCorePlugin());
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, Task<IApp>> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(provider => factory(provider).GetAwaiter().GetResult());
        collection.AddHostedService<SparkService>();
        return collection.AddSparkPlugin(new AspNetCorePlugin());
    }

    public static IServiceCollection AddSparkPlugin<TPlugin>(this IServiceCollection collection, TPlugin plugin) where TPlugin : class, IPlugin
    {
        collection.AddSingleton(plugin);
        collection.AddSingleton<IPlugin>(plugin);
        return collection.AddHostedService<SparkPluginService<TPlugin>>();
    }

    public static IServiceCollection AddSparkPlugin<TPlugin>(this IServiceCollection collection, Func<IServiceProvider, TPlugin> factory) where TPlugin : class, IPlugin
    {
        collection.AddSingleton(factory);
        collection.AddSingleton<IPlugin>(factory);
        return collection.AddHostedService<SparkPluginService<TPlugin>>();
    }
}