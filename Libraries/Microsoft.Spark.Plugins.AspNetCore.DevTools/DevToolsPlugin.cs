using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;
using Microsoft.Spark.Common.Text;
using Microsoft.Spark.Plugins.AspNetCore.DevTools.Models;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools;

[Plugin(name: "Microsoft.Spark.Plugins.AspNetCore.DevTools", version: "0.0.0")]
public class DevToolsPlugin : IAspNetCorePlugin
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

    internal MetaData MetaData => new() { Id = AppId, Name = AppName, Pages = _pages };
    internal readonly WebSocketCollection Sockets = [];

    private readonly ISenderPlugin _sender;
    private readonly IList<Page> _pages = [];

    public DevToolsPlugin(AspNetCorePlugin sender)
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

        builder.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "http error");
                throw new Exception(ex.Message, innerException: ex);
            }
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

    public async Task OnActivity(IApp app, IContext<IActivity> context)
    {
        Logger.Debug("OnActivity");
        await Sockets.Emit(Events.ActivityEvent.Received(
            context.Activity,
            context.Ref.Conversation
        ), context.CancellationToken);
    }

    public async Task OnActivitySent(IApp app, IActivity activity, IContext<IActivity> context)
    {
        Logger.Debug("OnActivitySent");
        await Sockets.Emit(
            Events.ActivityEvent.Sent(activity, context.Ref.Conversation),
            context.CancellationToken
        );
    }

    public async Task OnActivitySent(IApp app, ISenderPlugin sender, IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        Logger.Debug("OnActivitySent");
        await Sockets.Emit(
            Events.ActivityEvent.Sent(activity, reference.Conversation),
            cancellationToken
        );
    }

    public Task OnActivityResponse(IApp app, Response? response, IContext<IActivity> context)
    {
        return Task.Run(() => Logger.Debug("OnActivityResponse"));
    }

    public Task<Response> Do(IToken token, IActivity activity, CancellationToken cancellationToken = default)
    {
        return _sender.Do(token, activity, cancellationToken);
    }
}