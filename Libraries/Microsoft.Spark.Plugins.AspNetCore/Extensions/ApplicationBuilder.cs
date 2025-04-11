using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.Plugins.AspNetCore.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApp UseSpark(this IApplicationBuilder builder, bool routing = true)
    {
        var app = builder.ApplicationServices.GetService<IApp>() ?? new App(builder.ApplicationServices.GetService<IAppOptions>());
        var aspNetCore = builder.ApplicationServices.GetRequiredService<AspNetCorePlugin>();
        var plugins = builder.ApplicationServices.GetServices<IPlugin>();

        if (routing)
        {
            builder.UseRouting();
        }

        foreach (var plugin in plugins)
        {
            app.AddPlugin(plugin);

            if (!plugin.Equals(aspNetCore) && plugin is IAspNetCorePlugin aspNetCorePlugin)
            {
                aspNetCorePlugin.Configure(builder);
            }
        }

        return app;
    }

    public static AspNetCorePlugin GetAspNetCorePlugin(this IApplicationBuilder builder)
    {
        return builder.ApplicationServices.GetAspNetCorePlugin();
    }

    public static AspNetCorePlugin GetAspNetCorePlugin(this IServiceProvider provider)
    {
        return provider.GetRequiredService<AspNetCorePlugin>();
    }
}