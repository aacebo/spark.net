using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Events;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

[Plugin(name: "Microsoft.Spark.AspNetCore", version: "0.0.0")]
public class AspNetCorePlugin : ISender
{
    [AllowNull]
    [Dependency]
    public ILogger Logger { get; set; }

    public event IPlugin.ErrorEventHandler ErrorEvent = (_, _) => Task.Run(() => { });
    public event IPlugin.ActivityEventHandler ActivityEvent = (_, _) => Task.Run(() => (object?)null);

    public Task OnInit(IApp app)
    {
        return Task.Run(() => { });
    }

    public Task OnStart(IApp app, StartEventArgs args)
    {
        return Task.Run(() => Logger.Debug("OnStart"));
    }

    public Task OnError(IApp app, Apps.Events.ErrorEventArgs args)
    {
        return Task.Run(() => Logger.Debug("OnError"));
    }

    public Task OnActivity(IApp app, ISender plugin, ActivityEventArgs args)
    {
        throw new NotImplementedException();
    }

    public Task OnActivitySent(IApp app, ISender plugin, ActivitySentEventArgs args)
    {
        throw new NotImplementedException();
    }

    public Task OnActivityResponse(IApp app, ISender plugin, ActivityResponseEventArgs args)
    {
        throw new NotImplementedException();
    }

    public Task<IActivity> Send(IActivity activity, ConversationReference reference)
    {
        throw new NotImplementedException();
    }

    public IStreamer CreateStream(ConversationReference reference)
    {
        throw new NotImplementedException();
    }

    internal async Task<IResult> OnMessage(HttpContext context)
    {
        context.Request.EnableBuffering();
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        try
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault() ?? throw new UnauthorizedAccessException();
            var token = new JsonWebToken(authHeader.Replace("Bearer ", ""));

            var activity = await JsonSerializer.DeserializeAsync<IActivity>(context.Request.Body) ?? throw new BadHttpRequestException("could not read json activity payload");
            var res = await ActivityEvent(this, new()
            {
                Token = token,
                Activity = activity,
                Logger = Logger
            });

            return Results.Ok(res);
        }
        catch (Exception err)
        {
            Logger.Error(err);
            return Results.InternalServerError(err);
        }
    }
}