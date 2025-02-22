using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Message;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Events;
using Microsoft.Spark.Apps.Routing;
using Microsoft.Spark.AspNetCore;
using Microsoft.Spark.Extensions.Hosting;
using Microsoft.Spark.Extensions.Logging;

namespace Samples.Echo;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.AddSpark();

        var app = builder.Build();

        var spark = app.Services.GetService<IApp>();
        spark!.OnMessage(context =>
        {
            return Task.Run(() => { });
        });

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [Activity]
    public static void OnActivity(IContext<IActivity> context)
    {
        context.Logger.Info("on activity...");
    }

    [Message]
    public static void OnMessage(IContext<IMessageSendActivity> context)
    {
        context.Logger.Info("on message...");
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}