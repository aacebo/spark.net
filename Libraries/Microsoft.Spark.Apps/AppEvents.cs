using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Plugins;

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

            IContext<Activity> context = new Context<Activity>(
                sender,
                args.Token.AppId ?? "",
                Logger.Child(path),
                Api,
                args.Activity,
                reference
            );

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