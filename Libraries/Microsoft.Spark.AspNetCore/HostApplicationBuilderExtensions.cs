using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Spark.AspNetCore;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark();
        builder.Services.AddSingleton<Apps.IApp>(new Apps.App());
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Apps.IApp app)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark();
        builder.Services.AddSingleton(app);
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Apps.IAppBuilder app)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark();
        builder.Services.AddSingleton(app.Build());
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<Apps.IApp> @delegate)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark();
        builder.Services.AddSingleton(new Lazy<Apps.IApp>(@delegate));
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, Func<Task<Apps.IApp>> @delegate)
    {
        builder.Logging.AddSpark();
        builder.Services.AddSpark();
        builder.Services.AddSingleton(new Lazy<Apps.IApp>(() => @delegate().GetAwaiter().GetResult()));
        return builder;
    }
}