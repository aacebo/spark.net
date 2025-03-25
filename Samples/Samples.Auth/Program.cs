using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Events;
using Microsoft.Spark.Apps.Routing;
using Microsoft.Spark.AspNetCore;

namespace Samples.Auth;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseSpark();
        app.Run();
    }

    [Message("/signout", log: IContext.Property.Activity)]
    public static async Task OnSignOut(IContext<MessageActivity> context)
    {
        if (!context.IsSignedIn)
        {
            await context.Send("you are not signed in!");
            return;
        }

        await context.SignOut();
        await context.Send("you have been signed out!");
    }

    [Message(log: IContext.Property.Activity)]
    public static async Task OnMessage(IContext<MessageActivity> context)
    {
        if (!context.IsSignedIn)
        {
            await context.SignIn();
            return;
        }

        await context.Send("you are signed in!");
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}