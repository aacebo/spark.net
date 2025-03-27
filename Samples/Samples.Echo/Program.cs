using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Routing;
using Microsoft.Spark.AspNetCore;

namespace Samples.Echo;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOpenApi();
        builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [Activity]
    public static async Task OnActivity(IContext<Activity> context, [IContext.Next] IContext.Next next)
    {
        context.Log.Info(context.AppId);
        await next();
    }

    [Message(log: IContext.Property.Activity)]
    public static async Task OnMessage([IContext.Activity] MessageActivity activity, [IContext.Client] IContext.Client client)
    {
        await client.Typing();
        await client.Send($"you said '{activity.Text}'");
    }
}