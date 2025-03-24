using Microsoft.Spark.Api.Activities;
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

    [Message]
    public static void OnMessage([IContext.Logger] Microsoft.Spark.Common.Logging.ILogger logger, [IContext.Activity] MessageActivity activity)
    {
        logger.Info(activity);
        // await context.Typing();
        // await context.Send($"you said '{context.Activity.Text}'");
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}