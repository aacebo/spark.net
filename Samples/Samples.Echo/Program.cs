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

    // [Message("/signout", log: IContext.Property.Activity)]
    // public static async Task OnSignOut([IContext.Activity] MessageActivity activity, [IContext.Send] IContext.Send send)
    // {
    //     await send.Typing();
    //     await send.Text($"you said '{activity.Text}'");

    // }

    [Message("/signout", log: IContext.Property.Activity)]
    public static async Task OnSignOut(IContext<MessageActivity> context)
    {
        await context.SignOut();
    }

    [Message("/signin", log: IContext.Property.Activity)]
    public static async Task OnMessage(IContext<MessageActivity> context)
    {
        await context.SignIn();
    }

    [ErrorEvent]
    public static void OnEvent()
    {

    }
}