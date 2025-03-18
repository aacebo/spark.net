using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Message;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Events;
using Microsoft.Spark.Apps.Routing;
using Microsoft.Spark.AspNetCore;

namespace Samples.Echo;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.AddSpark();
        // builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        var spark = app.Services.GetService<IApp>();
        spark!.OnMessage(context => Task.Run(() => context.Logger.Info("message delegate...")));

        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [Activity]
    public static void OnActivity(IContext<Activity> context)
    {
        context.Logger.Info("on activity...");
    }

    [Message]
    public static void OnMessage(IContext<MessageActivity> context)
    {
        context.Logger.Info("on message...");
        context.Logger.Info(context.Activity);
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}