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
        collection.AddSingleton(app.Storage);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton<IApp>(app);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        return collection.AddSparkPlugin<AspNetCorePlugin>();
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IAppBuilder builder)
    {
        var app = builder.Build();
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton(app.Storage);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        return collection.AddSparkPlugin<AspNetCorePlugin>();
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IApp app)
    {
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton(app.Storage);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        return collection.AddSparkPlugin<AspNetCorePlugin>();
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, IApp> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSingleton(factory);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Logger);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Storage);
        return collection.AddSparkPlugin<AspNetCorePlugin>();
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, Task<IApp>> factory)
    {
        var log = new SparkLogger();

        collection.AddSingleton(log.Logger);
        collection.AddSingleton<ILoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSingleton(provider => factory(provider).GetAwaiter().GetResult());
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Logger);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Storage);
        return collection.AddSparkPlugin<AspNetCorePlugin>();
    }

    public static IServiceCollection AddSparkPlugin<TPlugin>(this IServiceCollection collection) where TPlugin : class, IPlugin
    {
        collection.AddSingleton<TPlugin>();
        collection.AddSingleton<IPlugin, TPlugin>(provider => provider.GetRequiredService<TPlugin>());
        return collection.AddHostedService<SparkPluginService<TPlugin>>();
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