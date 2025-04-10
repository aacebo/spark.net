using System.Diagnostics.CodeAnalysis;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Plugins.DevTools;

[Plugin(name: "Microsoft.Spark.Plugins.DevTools", version: "0.0.0")]
public class DevToolsPlugin : ISender
{
    [AllowNull]
    [Dependency]
    public ILogger Logger { get; set; }

    [Dependency("AppId", optional: true)]
    public string? AppId { get; set; }

    [Dependency("AppName", optional: true)]
    public string? AppName { get; set; }

    public event IPlugin.ErrorEventHandler ErrorEvent = (_, _) => Task.Run(() => { });
    public event IPlugin.ActivityEventHandler ActivityEvent = (_, _, _, _) => Task.FromResult(new Response(System.Net.HttpStatusCode.OK));

    private readonly IServiceProvider _services;

    public DevToolsPlugin(IServiceProvider provider)
    {
        _services = provider;
    }

    public Task OnInit(IApp app, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => { });
    }

    public Task OnStart(IApp app, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Logger.Debug("OnStart"));
    }

    public Task OnError(IApp app, IPlugin? plugin, Exception exception, IContext<IActivity>? context, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Logger.Debug("OnError"));
    }

    public Task OnActivity(IApp app, IContext<IActivity> context)
    {
        return Task.Run(() => Logger.Debug("OnActivity"));
    }

    public Task OnActivitySent(IApp app, IActivity activity, IContext<IActivity> context)
    {
        return Task.Run(() => Logger.Debug("OnActivitySent"));
    }

    public Task OnActivitySent(IApp app, ISender sender, IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Logger.Debug("OnActivitySent"));
    }

    public Task OnActivityResponse(IApp app, Response? response, IContext<IActivity> context)
    {
        return Task.Run(() => Logger.Debug("OnActivityResponse"));
    }

    public Task<IActivity> Send(IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return Send(activity, reference, cancellationToken);
    }

    public Task<TActivity> Send<TActivity>(TActivity activity, ConversationReference reference, CancellationToken cancellationToken = default) where TActivity : IActivity
    {
        throw new NotImplementedException();
    }

    public IStreamer CreateStream(ConversationReference reference, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> Do(IToken token, IActivity activity, CancellationToken cancellationToken = default)
    {
        try
        {
            var res = await ActivityEvent(this, token, activity, cancellationToken);
            Logger.Debug(res);
            return res;
        }
        catch (Exception err)
        {
            Logger.Error(err);
            await ErrorEvent(this, err);
            return new Response(System.Net.HttpStatusCode.InternalServerError, err.ToString());
        }
    }
}