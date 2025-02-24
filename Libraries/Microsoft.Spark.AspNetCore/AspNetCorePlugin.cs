using System.Reflection;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public class AspNetCorePlugin : IPlugin
{
    public string Name { get; } = "Microsoft.Spark.AspNetCore";

    public event IPlugin.ErrorEventHandler ErrorEvent = (_, _) => Task.Run(() => { });
    public event IPlugin.StartEventHandler StartEvent = (_, _) => Task.Run(() => { });
    public event IPlugin.ActivityReceivedEventHandler ActivityReceivedEvent = (_, _) => Task.Run(() => (object?)null);

    protected ILogger _logger;

    public AspNetCorePlugin(ILogger? logger = null)
    {
        logger ??= new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        _logger = logger.Child(Name);
    }

    public Task OnInit(IApp app)
    {
        return Task.Run(() => _logger = app.Logger.Child(Name));
    }

    public Task OnStart(IApp app, Apps.Events.StartEventArgs args)
    {
        return Task.Run(() => _logger.Debug("OnStart"));
    }

    public Task OnActivity(IApp app, Apps.Events.ActivityReceivedEventArgs activity)
    {
        return Task.Run(() => _logger.Debug("OnActivity"));
    }

    public Task OnError(IApp app, Apps.Events.ErrorEventArgs args)
    {
        return Task.Run(() => _logger.Debug("OnError"));
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
            var res = await ActivityReceivedEvent(this, new()
            {
                Token = token,
                Activity = activity,
                Logger = _logger
            });

            return Results.Ok(res);
        }
        catch (Exception err)
        {
            _logger.Error(err);
            return Results.InternalServerError(err);
        }
    }
}