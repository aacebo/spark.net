using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;
using Microsoft.Spark.Common.Text;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools;

[Plugin(name: "Microsoft.Spark.Plugins.AspNetCore.DevTools", version: "0.0.0")]
public class DevToolsPlugin : ISenderPlugin, IAspNetCorePlugin
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

    private readonly ISenderPlugin _sender;
    private readonly IList<Page> _pages = [];

    public DevToolsPlugin(ISenderPlugin sender)
    {
        _sender = sender;
    }

    public IApplicationBuilder Configure(IApplicationBuilder builder)
    {
        builder.UseWebSockets(new WebSocketOptions()
        {
            AllowedOrigins = { "*" }
        });

        builder.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "web")),
            ServeUnknownFileTypes = true,
            RequestPath = "/devtools"
        });

        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/v3/conversations/activities", _ =>
            {
                return Task.Run(() => { });
            });

            endpoints.MapGet("/devtools/sockets", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    await Results.BadRequest().ExecuteAsync(context);
                    return;
                }

                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            });
        });

        return builder;
    }

    public DevToolsPlugin AddPage(Page page)
    {
        _pages.Add(page);
        return this;
    }

    public Task OnInit(IApp app, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            Logger.Warn(
                new StringBuilder()
                    .Bold(
                        new StringBuilder()
                            .Yellow("⚠️  Devtools are not secure and should not be used production environments ⚠️")
                            .ToString()
                    )
            );
        });
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

    public Task OnActivitySent(IApp app, ISenderPlugin sender, IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => Logger.Debug("OnActivitySent"));
    }

    public Task OnActivityResponse(IApp app, Response? response, IContext<IActivity> context)
    {
        return Task.Run(() => Logger.Debug("OnActivityResponse"));
    }

    public Task<IActivity> Send(IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return _sender.Send(activity, reference, cancellationToken);
    }

    public Task<TActivity> Send<TActivity>(TActivity activity, ConversationReference reference, CancellationToken cancellationToken = default) where TActivity : IActivity
    {
        return _sender.Send(activity, reference, cancellationToken);
    }

    public IStreamer CreateStream(ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return _sender.CreateStream(reference, cancellationToken);
    }

    public Task<Response> Do(IToken token, IActivity activity, CancellationToken cancellationToken = default)
    {
        return _sender.Do(token, activity, cancellationToken);
    }
}