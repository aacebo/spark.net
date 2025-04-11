using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Extensions;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Plugins.AspNetCore;

[Plugin(name: "Microsoft.Spark.Plugins.AspNetCore", version: "0.0.0")]
public partial class AspNetCorePlugin : ISenderPlugin, IAspNetCorePlugin
{
    [AllowNull]
    [Dependency]
    public ILogger Logger { get; set; }

    [AllowNull]
    [Dependency]
    public IHttpClient Client { get; set; }

    [Dependency("BotToken", optional: true)]
    public IToken? BotToken { get; set; }

    public event IPlugin.ErrorEventHandler ErrorEvent = (_, _) => Task.Run(() => { });
    public event IPlugin.ActivityEventHandler ActivityEvent = (_, _, _, _) => Task.FromResult(new Response(System.Net.HttpStatusCode.OK));

    private SparkContext Context => _services.GetRequiredService<SparkContext>();
    private readonly IServiceProvider _services;

    public AspNetCorePlugin(IServiceProvider provider)
    {
        _services = provider;
    }

    public IApplicationBuilder Configure(IApplicationBuilder builder)
    {
        builder.UseRouting();
        builder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return builder;
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
        Context.Activity = context;
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
        Context.Response = response;
        return Task.Run(() => Logger.Debug("OnActivityResponse"));
    }

    public async Task<IActivity> Send(IActivity activity, ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return await Send<IActivity>(activity, reference, cancellationToken);
    }

    public async Task<TActivity> Send<TActivity>(TActivity activity, ConversationReference reference, CancellationToken cancellationToken = default) where TActivity : IActivity
    {
        var client = new ApiClient(reference.ServiceUrl, Client, cancellationToken);

        if (activity.Id != null && !activity.IsStreaming)
        {
            await client
                .Conversations
                .Activities
                .UpdateAsync(reference.Conversation.Id, activity.Id, activity);

            return activity;
        }

        var res = await client
            .Conversations
            .Activities
            .CreateAsync(reference.Conversation.Id, activity);

        activity.Id = res?.Id;
        return activity;
    }

    public IStreamer CreateStream(ConversationReference reference, CancellationToken cancellationToken = default)
    {
        return new Stream()
        {
            Send = async activity =>
            {
                var res = await Send(activity, reference, cancellationToken);
                return res;
            }
        };
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