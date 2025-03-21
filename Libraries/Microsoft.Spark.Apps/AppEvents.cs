using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public IApp OnError(ErrorEventHandler handler);
    public IApp OnStart(StartEventHandler handler);
    public IApp OnActivity(ActivityEventHandler handler);

    public delegate Task ErrorEventHandler(IApp app, Events.ErrorEventArgs args);
    public delegate Task StartEventHandler(IApp app, Events.StartEventArgs args);
    public delegate Task<object?> ActivityEventHandler(IApp app, ISender plugin, Events.ActivityEventArgs args);
}

public partial class App
{
    protected event IApp.ErrorEventHandler ErrorEvent;
    protected event IApp.StartEventHandler StartEvent;
    protected event IApp.ActivityEventHandler ActivityEvent;

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

    protected Task OnErrorEvent(Events.ErrorEventArgs args)
    {
        return Task.Run(() => args.Logger.Error(args.Error));
    }

    protected Task OnStartEvent(Events.StartEventArgs args)
    {
        return Task.Run(() => args.Logger.Info("started"));
    }

    protected async Task<object?> OnActivityEvent(ISender sender, Events.ActivityEventArgs args)
    {
        var routes = Router.Select(args.Activity);

        try
        {
            IContext<Activity> context = new Context<Activity>(sender)
            {
                Activity = args.Activity,
                AppId = args.Token.AppId ?? "",
                Log = Logger,
                Api = Api,
                Ref = new()
                {
                    ServiceUrl = args.Activity.ServiceUrl ?? args.Token.ServiceUrl,
                    ChannelId = args.Activity.ChannelId,
                    Bot = args.Activity.Recipient,
                    User = args.Activity.From,
                    Locale = args.Activity.Locale,
                    Conversation = args.Activity.Conversation,
                }
            };

            foreach (var route in routes)
            {
                await route.Invoke(context);
            }
        }
        catch (Exception err)
        {
            await ErrorEvent(this, new()
            {
                Error = err,
                Logger = Logger
            });
        }

        return null;
    }
}