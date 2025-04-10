using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Extensions;

namespace Microsoft.Spark.Plugins.AspNetCore;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder)
    {
        builder.AddSparkCore();
        builder.AddSparkPlugin<AspNetCorePlugin>();
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IApp app)
    {
        builder.AddSparkCore(app);
        builder.AddSparkPlugin<AspNetCorePlugin>();
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IAppOptions options)
    {
        builder.AddSparkCore(options);
        builder.AddSparkPlugin<AspNetCorePlugin>();
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }

    public static IHostApplicationBuilder AddSpark(this IHostApplicationBuilder builder, IAppBuilder appBuilder)
    {
        builder.AddSparkCore(appBuilder);
        builder.AddSparkPlugin<AspNetCorePlugin>();
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }
}