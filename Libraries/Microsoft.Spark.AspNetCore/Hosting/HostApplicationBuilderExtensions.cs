using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder)
    {
        var settings = builder.Configuration.GetSpark();

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark();
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IApp app)
    {
        var settings = builder.Configuration.GetSpark();

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IAppOptions options)
    {
        var settings = builder.Configuration.GetSpark();

        // client credentials
        if (options.Credentials == null && settings.ClientId != null && settings.ClientSecret != null)
        {
            options.Credentials = new ClientCredentials(
                settings.ClientId,
                settings.ClientSecret,
                settings.TenantId
            );
        }

        options.Logger ??= new ConsoleLogger(settings.Logging);
        var app = new App(options);

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IAppBuilder appBuilder)
    {
        var settings = builder.Configuration.GetSpark();

        // client credentials
        if (settings.ClientId != null && settings.ClientSecret != null)
        {
            appBuilder = appBuilder.AddCredentials(new ClientCredentials(
                settings.ClientId,
                settings.ClientSecret,
                settings.TenantId
            ));
        }

        var app = appBuilder.Build();

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<IServiceProvider, IApp> factory)
    {
        var settings = builder.Configuration.GetSpark();

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark();
        builder.Services.AddSpark(factory);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<IServiceProvider, Task<IApp>> factory)
    {
        var settings = builder.Configuration.GetSpark();

        builder.Services.AddSingleton(settings);
        builder.Logging.AddSpark();
        builder.Services.AddSpark(factory);
        return builder;
    }

    public static IHostApplicationBuilder AddSparkPlugin<TPlugin>(this IHostApplicationBuilder builder) where TPlugin : class, IPlugin
    {
        builder.Services.AddSparkPlugin<TPlugin>();
        return builder;
    }

    public static IHostApplicationBuilder AddSparkPlugin<TPlugin>(this IHostApplicationBuilder builder, TPlugin plugin) where TPlugin : class, IPlugin
    {
        builder.Services.AddSparkPlugin(plugin);
        return builder;
    }

    public static IHostApplicationBuilder AddSparkPlugin<TPlugin>(this IHostApplicationBuilder builder, Func<IServiceProvider, Task<TPlugin>> factory) where TPlugin : class, IPlugin
    {
        builder.Services.AddSparkPlugin(provider => factory(provider).GetAwaiter().GetResult());
        return builder;
    }
}