using Microsoft.Spark.Apps;
using Microsoft.Spark.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddSpark(App.Builder().AddLogger(level: Microsoft.Spark.Common.Logging.LogLevel.Debug));

var app = builder.Build();
var spark = app.Services.GetService<IApp>()!;

spark.OnMessage("/signout", async context =>
{
    if (!context.IsSignedIn)
    {
        await context.Send("you are not signed in!");
        return null;
    }

    await context.SignOut();
    await context.Send("you have been signed out!");
    return null;
});

spark.OnMessage(async context =>
{
    if (!context.IsSignedIn)
    {
        await context.SignIn();
        return null;
    }

    await context.Send("you are signed in!");
    return null;
});

app.UseSpark();
app.Run();