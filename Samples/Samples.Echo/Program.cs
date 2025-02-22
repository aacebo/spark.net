using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Message;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Routes;
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

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [OnActivity]
    public static void OnActivity(IActivityContext<IActivity> context)
    {
        context.Logger.Info("on activity...");
    }

    [OnMessage]
    public static void OnMessage(IActivityContext<IMessageSendActivity> context)
    {
        context.Logger.Info("on message...");
    }
}