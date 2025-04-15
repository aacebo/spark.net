using Microsoft.Teams.Api.Activities;
using Microsoft.Teams.Apps;
using Microsoft.Teams.Apps.Extensions;
using Microsoft.Teams.Apps.Routing;
using Microsoft.Teams.Plugins.AspNetCore.DevTools.Extensions;
using Microsoft.Teams.Plugins.AspNetCore.Extensions;

namespace Samples.Echo;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddOpenApi();
        builder.AddTeams().AddTeamsDevTools();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseTeams();
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