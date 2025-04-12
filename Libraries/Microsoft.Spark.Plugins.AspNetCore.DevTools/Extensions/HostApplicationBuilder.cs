using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Spark.Apps.Extensions;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddSparkDevTools(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(builder.Configuration.GetSparkDevTools());
        builder.AddSparkPlugin<DevToolsPlugin>();
        builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
        return builder;
    }
}