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
}