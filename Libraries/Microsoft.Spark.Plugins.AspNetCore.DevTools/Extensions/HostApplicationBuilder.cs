using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Apps.Extensions;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSparkDevTools(this IHostApplicationBuilder builder, ISenderPlugin sender)
    {
        builder.Services.AddSingleton(builder.Configuration.GetSparkDevTools());
        builder.AddSparkPlugin(new DevToolsPlugin(sender));
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }

    public static IHostApplicationBuilder AddSparkDevTools<TPlugin>(this IHostApplicationBuilder builder) where TPlugin : ISenderPlugin
    {
        builder.Services.AddSingleton(builder.Configuration.GetSparkDevTools());
        builder.Services.AddSparkPlugin(provider =>
        {
            var sender = provider.GetRequiredService<TPlugin>();
            return new DevToolsPlugin(sender);
        });

        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }

    public static IHostApplicationBuilder AddSparkDevTools(this IHostApplicationBuilder builder)
    {
        builder.AddSparkDevTools<AspNetCorePlugin>();
        return builder;
    }
}