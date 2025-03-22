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

        var spark = app.Services.GetService<IApp>();
        spark!.OnMessage(context => Task.Run(() => context.Log.Info("message delegate...")));

        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [Message]
    public static async Task OnMessage(IContext<MessageActivity> context)
    {
        await context.Typing();
        context.Log.Info(context.Activity);
        await context.Send($"you said '{context.Activity.Text}'");
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}