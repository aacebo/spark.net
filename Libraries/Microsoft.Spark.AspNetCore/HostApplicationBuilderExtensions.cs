using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder)
    {
        var app = new App();

        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IApp app)
    {
        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IAppBuilder appBuilder)
    {
        var app = appBuilder.Build();

        builder.Logging.AddSpark(app.Logger);
        builder.Services.AddSpark(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<IServiceProvider, IApp> factory)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark(factory);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<IServiceProvider, Task<IApp>> factory)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark(factory);
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