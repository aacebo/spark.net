using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Extensions.Hosting;
using Microsoft.Spark.Extensions.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSpark(this IServiceCollection collection)
    {
        collection.AddSingleton<Common.Logging.ConsoleLogger>();
        collection.AddSingleton<Common.Logging.ILogger, Common.Logging.ConsoleLogger>(provider => provider.GetRequiredService<Common.Logging.ConsoleLogger>());
        collection.AddSingleton<Common.Storage.LocalStorage<object>>();
        collection.AddSingleton<Common.Storage.IStorage<string, object>>(provider => provider.GetRequiredService<Common.Storage.LocalStorage<object>>());

        collection.AddSingleton<SparkLogger>();
        collection.AddSingleton<ILogger, SparkLogger>(provider => provider.GetRequiredService<SparkLogger>());
        collection.AddSingleton<ILoggerFactory, LoggerFactory>(provider =>
        {
            var logger = provider.GetRequiredService<SparkLogger>();
            return new LoggerFactory([new SparkLoggerProvider(logger)]);
        });

        collection.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<SparkSettings>();
            var logger = provider.GetRequiredService<Common.Logging.ILogger>();
            return App.Builder(settings.Apply()).AddLogger(logger).Build();
        });

        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
    }

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
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IAppBuilder builder)
    {
        var app = builder.Build();
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton(app.Storage);
        collection.AddSingleton<ILoggerFactory, LoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, IApp app)
    {
        var log = new SparkLogger(app.Logger);

        collection.AddSingleton(app.Logger);
        collection.AddSingleton(app.Storage);
        collection.AddSingleton<ILoggerFactory, LoggerFactory>(_ => new LoggerFactory([new SparkLoggerProvider(log)]));
        collection.AddSingleton<ILogger>(log);
        collection.AddSingleton(app);
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, IApp> factory)
    {
        collection.AddSingleton(provider => provider.GetRequiredService<Common.Logging.ILogger>());
        collection.AddSingleton<ILoggerFactory, LoggerFactory>();
        collection.AddSingleton<ILogger, SparkLogger>(provider => provider.GetRequiredService<SparkLogger>());
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSingleton(factory);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Logger);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Storage);
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
    }

    public static IServiceCollection AddSpark(this IServiceCollection collection, Func<IServiceProvider, Task<IApp>> factory)
    {
        collection.AddSingleton(provider => provider.GetRequiredService<Common.Logging.ILogger>());
        collection.AddSingleton<ILoggerFactory, LoggerFactory>();
        collection.AddSingleton<ILogger, SparkLogger>(provider => provider.GetRequiredService<SparkLogger>());
        collection.AddHostedService<SparkService>();
        collection.AddScoped<SparkHttpContext>();
        collection.AddTransient(provider => provider.GetRequiredService<SparkHttpContext>().Activity);
        collection.AddSingleton(provider => factory(provider).GetAwaiter().GetResult());
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Logger);
        collection.AddSingleton(provider => provider.GetRequiredService<IApp>().Storage);
        collection.AddSparkPlugin<AspNetCorePlugin>();
        collection.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return collection;
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
        collection.AddSingleton<IPlugin>(provider => provider.GetRequiredService<TPlugin>());
        return collection.AddHostedService<SparkPluginService<TPlugin>>();
    }

    public static IServiceCollection AddSparkPlugin<TPlugin>(this IServiceCollection collection, Func<IServiceProvider, TPlugin> factory) where TPlugin : class, IPlugin
    {
        collection.AddSingleton(factory);
        collection.AddSingleton<IPlugin>(factory);
        return collection.AddHostedService<SparkPluginService<TPlugin>>();
    }
}