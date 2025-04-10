using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;
using Microsoft.Spark.Extensions.Logging;

namespace Microsoft.Spark.Apps.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSparkCore(this IHostApplicationBuilder builder)
    {
        return AddSparkCore(builder, new AppOptions());
    }

    public static IHostApplicationBuilder AddSparkCore(this IHostApplicationBuilder builder, IApp app)
    {
        builder.Services.AddSingleton(builder.Configuration.GetSpark());
        builder.Services.AddSingleton(builder.Configuration.GetSparkLogging());
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSparkCore(this IHostApplicationBuilder builder, IAppOptions options)
    {
        var settings = builder.Configuration.GetSpark();
        var loggingSettings = builder.Configuration.GetSparkLogging();

        // client credentials
        if (options.Credentials == null && settings.ClientId != null && settings.ClientSecret != null)
        {
            options.Credentials = new ClientCredentials(
                settings.ClientId,
                settings.ClientSecret,
                settings.TenantId
            );
        }

        options.Logger ??= new ConsoleLogger(loggingSettings);
        var app = new App(options);

        builder.Services.AddSingleton(settings);
        builder.Services.AddSingleton(loggingSettings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSparkCore(this IHostApplicationBuilder builder, IAppBuilder appBuilder)
    {
        var settings = builder.Configuration.GetSpark();
        var loggingSettings = builder.Configuration.GetSparkLogging();

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
        builder.Services.AddSingleton(loggingSettings);
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
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
}