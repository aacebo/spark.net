using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public IApp OnError(ErrorEventHandler handler);
    public IApp OnStart(StartEventHandler handler);
    public IApp OnActivity(ActivityEventHandler handler);
    public IApp OnActivitySent(ActivitySentEventHandler handler);
    public IApp OnActivityResponse(ActivityResponseEventHandler handler);

    public delegate Task ErrorEventHandler(IApp app, Events.ErrorEventArgs args);
    public delegate Task StartEventHandler(IApp app, Events.StartEventArgs args);
    public delegate Task<Response?> ActivityEventHandler(IApp app, ISender plugin, Events.ActivityEventArgs args);
    public delegate Task ActivitySentEventHandler(IApp app, ISender plugin, Events.ActivitySentEventArgs args);
    public delegate Task ActivityResponseEventHandler(IApp app, ISender plugin, Events.ActivityResponseEventArgs args);
}

public partial class App
{
    protected event IApp.ErrorEventHandler ErrorEvent;
    protected event IApp.StartEventHandler StartEvent;
    protected event IApp.ActivityEventHandler ActivityEvent;
    protected event IApp.ActivitySentEventHandler ActivitySentEvent;
    protected event IApp.ActivityResponseEventHandler ActivityResponseEvent;

    public IApp OnError(IApp.ErrorEventHandler handler)
    {
        ErrorEvent += handler;
        return this;
    }

    public IApp OnStart(IApp.StartEventHandler handler)
    {
        StartEvent += handler;
        return this;
    }

    public IApp OnActivity(IApp.ActivityEventHandler handler)
    {
        ActivityEvent += handler;
        return this;
    }

    public IApp OnActivitySent(IApp.ActivitySentEventHandler handler)
    {
        ActivitySentEvent += handler;
        return this;
    }

    public IApp OnActivityResponse(IApp.ActivityResponseEventHandler handler)
    {
        ActivityResponseEvent += handler;
        return this;
    }

    protected async Task OnErrorEvent(Events.ErrorEventArgs args)
    {
        args.Logger.Error(args.Error);

        if (args.Error is HttpException ex)
        {
            args.Logger.Error(ex.Request?.RequestUri?.ToString());
            args.Logger.Error(await ex.Request!.Content!.ReadAsStringAsync());
        }

        foreach (var plugin in Plugins)
        {
            await plugin.OnError(this, args);
        }
    }

    protected Task OnStartEvent(Events.StartEventArgs args)
    {
        return Task.Run(() => args.Logger.Info("started"));
    }

    protected async Task OnActivitySentEvent(ISender sender, Events.ActivitySentEventArgs args)
    {
        Logger.Debug(args.Activity);

        foreach (var plugin in Plugins)
        {
            await plugin.OnActivitySent(this, sender, args);
        }
    }

    protected async Task OnActivityResponseEvent(ISender sender, Events.ActivityResponseEventArgs args)
    {
        Logger.Debug(args.Response);

        foreach (var plugin in Plugins)
        {
            await plugin.OnActivityResponse(this, sender, args);
        }
    }

    protected async Task<Response?> OnActivityEvent(ISender sender, Events.ActivityEventArgs args)
    {
        var routes = Router.Select(args.Activity);

        try
        {
            JsonWebToken? userToken = null;

            try
            {
                var tokenResponse = await Api.Users.Token.GetAsync(new()
                {
                    UserId = args.Activity.From.Id,
                    ChannelId = args.Activity.ChannelId,
                    ConnectionName = "graph"
                });

                userToken = new JsonWebToken(tokenResponse);
            }
            catch { }

            foreach (var plugin in Plugins)
            {
                await plugin.OnActivity(this, sender, args);
            }

            var path = args.Activity.GetPath();
            Logger.Debug(path);

            var reference = new ConversationReference()
            {
                ServiceUrl = args.Activity.ServiceUrl ?? args.Token.ServiceUrl,
                ChannelId = args.Activity.ChannelId,
                Bot = args.Activity.Recipient,
                User = args.Activity.From,
                Locale = args.Activity.Locale,
                Conversation = args.Activity.Conversation,
            };

            var userGraphTokenProvider = Azure.Core.DelegatedTokenCredential.Create((context, _) =>
            {
                return userToken == null ? default : new Azure.Core.AccessToken(userToken.ToString(), userToken.Token.ValidTo);
            });

            object? data = null;
            var i = -1;
            async Task<object?> next(IContext<IActivity> context)
            {
                if (i == routes.Count - 1) return data;
                i++;
                var res = await routes[i].Invoke(context);

                if (res != null)
                    data = res;

                return res;
            }

            var stream = sender.CreateStream(reference);
            stream.OnChunk += activity =>
            {
                ActivitySentEvent(this, sender, new()
                {
                    Activity = activity,
                    Bot = reference.Bot,
                    ChannelId = reference.ChannelId,
                    Conversation = reference.Conversation,
                    ServiceUrl = reference.ServiceUrl,
                    ActivityId = reference.ActivityId,
                    Locale = reference.Locale,
                    User = reference.User
                });
            };

            var context = new Context<IActivity>(sender, stream)
            {
                AppId = args.Token.AppId ?? Id ?? string.Empty,
                Log = Logger.Child(path),
                Storage = Storage,
                Api = Api,
                Activity = args.Activity,
                Ref = reference,
                IsSignedIn = userToken != null,
                OnNext = next,
                UserGraph = new Graph.GraphServiceClient(userGraphTokenProvider),
                OnActivitySent = (sender, args) => ActivitySentEvent(this, sender, args)
            };

            var res = await next(context);

            if (res != null)
            {
                var response = res is Response value ? value : new Response(System.Net.HttpStatusCode.OK, res);
                await ActivityResponseEvent(this, sender, new()
                {
                    Activity = context.Activity,
                    Response = response,
                    Bot = reference.Bot,
                    ChannelId = reference.ChannelId,
                    Conversation = reference.Conversation,
                    ServiceUrl = reference.ServiceUrl,
                    ActivityId = reference.ActivityId,
                    Locale = reference.Locale,
                    User = reference.User
                });

                return response;
            }

            return null;
        }
        catch (Exception err)
        {
            await ErrorEvent(this, new()
            {
                Error = err,
                Logger = Logger,
                Activity = args.Activity
            });

            return new Response(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}