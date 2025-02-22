using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSpark(this IApplicationBuilder builder)
    {
        var app = builder.ApplicationServices.GetService<IApp>() ?? new App(builder.ApplicationServices.GetService<IAppOptions>());

        builder.UseRouting();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async (context) =>
            {
                await Task.FromResult("hello world");
            });

            endpoints.MapPost("/api/messages", async (context) =>
            {
                await Task.Run(() => app.Logger.Info("new message..."));
            });
        });

        return builder;
    }
}