using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApp UseSpark(this IApplicationBuilder builder, bool routing = true)
    {
        var app = builder.ApplicationServices.GetService<IApp>() ?? new App(builder.ApplicationServices.GetService<IAppOptions>());
        var aspNetCore = builder.ApplicationServices.GetRequiredService<AspNetCorePlugin>();
        var plugins = builder.ApplicationServices.GetServices<IPlugin>();

        foreach (var plugin in plugins)
        {
            app.AddPlugin(plugin);
        }

        if (routing)
        {
            builder.UseRouting();
            builder.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        return app;
    }

    public static AspNetCorePlugin GetAspNetCorePlugin(this IApplicationBuilder builder)
    {
        return builder.ApplicationServices.GetRequiredService<AspNetCorePlugin>();
    }
}