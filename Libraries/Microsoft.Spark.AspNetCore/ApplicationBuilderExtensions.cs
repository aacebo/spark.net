using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSpark(this IApplicationBuilder builder)
    {
        var app = builder.ApplicationServices.GetService<IApp>() ?? new App(builder.ApplicationServices.GetService<IAppOptions>());
        var aspNetCore = builder.ApplicationServices.GetService<AspNetCorePlugin>();
        var plugins = builder.ApplicationServices.GetServices<IPlugin>();

        if (aspNetCore == null)
            throw new ArgumentNullException("`AspNetCorePlugin` not found");

        foreach (var plugin in plugins)
            app.AddPlugin(plugin);

        builder.UseRouting();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/api/messages", async (context) => await aspNetCore.OnMessage(context));
        });

        return builder;
    }
}