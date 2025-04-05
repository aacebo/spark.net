using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSpark(this IApplicationBuilder builder)
    {
        var app = builder.ApplicationServices.GetService<IApp>() ?? new App(builder.ApplicationServices.GetService<IAppOptions>());
        var aspNetCore = builder.ApplicationServices.GetService<AspNetCorePlugin>() ?? throw new ArgumentNullException("`AspNetCorePlugin` not found");
        var plugins = builder.ApplicationServices.GetServices<IPlugin>();

        foreach (var plugin in plugins)
        {
            app.AddPlugin(plugin);
        }

        builder.UseRouting();
        builder.UseMiddleware<SparkContextMiddleware>();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/api/messages", async (context) =>
            {
                var res = await aspNetCore.OnMessage(context);
                await res.ExecuteAsync(context);
            });
        });

        return builder;
    }
}