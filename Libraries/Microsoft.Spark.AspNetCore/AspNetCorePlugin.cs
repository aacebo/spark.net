using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

[Plugin(name: "Microsoft.Spark.AspNetCore", version: "0.0.0")]
public partial class AspNetCorePlugin : ISender
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
    public event IPlugin.ActivityEventHandler ActivityEvent = (_, _, _) => Task.FromResult<Response?>(null);

    private SparkHttpContext Context => _services.GetRequiredService<SparkHttpContext>();
    private readonly IServiceProvider _services;

    public AspNetCorePlugin(IServiceProvider provider)
    {
        _services = provider;
    }

    public Task OnInit(IApp app)
    {
        return Task.Run(() => { });
    }

    public Task OnStart(IApp app)
    {
        return Task.Run(() => Logger.Debug("OnStart"));
    }

    public Task OnError(IApp app, IPlugin? plugin, Exception exception, IContext<IActivity>? context)
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

    public Task OnActivitySent(IApp app, ISender sender, IActivity activity, ConversationReference reference)
    {
        return Task.Run(() => Logger.Debug("OnActivitySent"));
    }

    public Task OnActivityResponse(IApp app, Response? response, IContext<IActivity> context)
    {
        Context.Response = response;
        return Task.Run(() => Logger.Debug("OnActivityResponse"));
    }

    public async Task<IActivity> Send(IActivity activity, ConversationReference reference)
    {
        return await Send<IActivity>(activity, reference);
    }

    public async Task<TActivity> Send<TActivity>(TActivity activity, ConversationReference reference) where TActivity : IActivity
    {
        var client = new ApiClient(reference.ServiceUrl, Client);

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

    public IStreamer CreateStream(ConversationReference reference)
    {
        return new Stream()
        {
            Send = async activity =>
            {
                var res = await Send(activity, reference);
                return res;
            }
        };
    }

    internal async Task<IResult> OnMessage(HttpContext context)
    {
        context.Request.EnableBuffering();
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        try
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault() ?? throw new UnauthorizedAccessException();
            var token = new JsonWebToken(authHeader.Replace("Bearer ", ""));
            var activity = await JsonSerializer.DeserializeAsync<Activity>(context.Request.Body) ?? throw new BadHttpRequestException("could not read json activity payload");
            var sparkContext = context.RequestServices.GetRequiredService<SparkHttpContext>();
            sparkContext.Token = token;

            var res = await ActivityEvent(this, token, activity) ?? new Response(System.Net.HttpStatusCode.OK);
            Logger.Debug(res);

            return TypedResults.Json(res.Body, statusCode: (int)res.Status);
        }
        catch (Exception err)
        {
            Logger.Error(err);
            await ErrorEvent(this, err);
            return TypedResults.InternalServerError(err.ToString());
        }
    }
}