using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

internal delegate Task ActivitySentEventHandler(ISender plugin, Events.ActivitySentEventArgs args);

public partial interface IContext<TActivity> where TActivity : IActivity
{
    /// <summary>
    /// the app id of the bot
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// the app logger instance
    /// </summary>
    public ILogger Log { get; set; }

    /// <summary>
    /// the api client
    /// </summary>
    public ApiClient Api { get; set; }

    /// <summary>
    /// the inbound activity
    /// </summary>
    public TActivity Activity { get; set; }

    /// <summary>
    /// the inbound activity conversation reference
    /// </summary>
    public ConversationReference Ref { get; set; }

    /// <summary>
    /// the users graph client
    /// </summary>
    public Graph.GraphServiceClient UserGraph { get; set; }

    /// <summary>
    /// any extra data
    /// </summary>
    public IDictionary<string, object> Extra { get; set; }

    /// <summary>
    /// destruct the context
    /// </summary>
    /// <param name="log">the ILogger instance</param>
    /// <param name="api">the api client</param>
    /// <param name="activity">the inbound activity</param>
    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity);

    /// <summary>
    /// destruct the context
    /// </summary>
    /// <param name="log">the ILogger instance</param>
    /// <param name="api">the api client</param>
    /// <param name="activity">the inbound activity</param>
    /// <param name="send">the methods to send activities</param>
    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity, out IContext.Send send);

    /// <summary>
    /// destruct the context
    /// </summary>
    /// <param name="appId">the apps id</param>
    /// <param name="log">the ILogger instance</param>
    /// <param name="api">the api client</param>
    /// <param name="activity">the inbound activity</param>
    /// <param name="reference">the inbound conversation reference</param>
    /// <param name="send">the methods to send activities</param>
    public void Deconstruct(out string appId, out ILogger log, out ApiClient api, out TActivity activity, out ConversationReference reference, out IContext.Send send);

    /// <summary>
    /// send an activity to the conversation
    /// </summary>
    /// <param name="activity">activity activity to send</param>
    public Task<T> Send<T>(T activity) where T : IActivity;

    /// <summary>
    /// send a message activity to the conversation
    /// </summary>
    /// <param name="text">the text to send</param>
    public Task<MessageActivity> Send(string text);

    /// <summary>
    /// send a message activity with a card attachment
    /// </summary>
    /// <param name="card">the card to send as an attachment</param>
    public Task<MessageActivity> Send(Cards.Card card);

    /// <summary>
    /// send a typing activity
    /// </summary>
    public Task<TypingActivity> Typing();

    /// <summary>
    /// convert the context to that of another activity type
    /// </summary>
    public IContext<IActivity> ToActivityType();

    /// <summary>
    /// convert the context to that of another activity type
    /// </summary>
    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : IActivity;
}

public partial class Context<TActivity> : IContext<TActivity> where TActivity : IActivity
{
    public required ISender Sender { get; set; }
    public required string AppId { get; set; }
    public required ILogger Log { get; set; }
    public required ApiClient Api { get; set; }
    public required TActivity Activity { get; set; }
    public required ConversationReference Ref { get; set; }
    public required Graph.GraphServiceClient UserGraph { get; set; }
    public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();

    internal ActivitySentEventHandler ActivitySentEvent { get; set; } = (_, _) => Task.Run(() => { });

    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity)
    {
        log = Log;
        api = Api;
        activity = Activity;
    }

    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity, out IContext.Send send)
    {
        log = Log;
        api = Api;
        activity = Activity;
        send = new IContext.Send(ToActivityType());
    }

    public void Deconstruct(out string appId, out ILogger log, out ApiClient api, out TActivity activity, out ConversationReference reference, out IContext.Send send)
    {
        appId = AppId;
        log = Log;
        api = Api;
        activity = Activity;
        reference = Ref;
        send = new IContext.Send(ToActivityType());
    }

    public async Task<T> Send<T>(T activity) where T : IActivity
    {
        var res = await Sender.Send(activity, Ref);

        await ActivitySentEvent(Sender, new()
        {
            Activity = res,
            Bot = Ref.Bot,
            ChannelId = Ref.ChannelId,
            Conversation = Ref.Conversation,
            ServiceUrl = Ref.ServiceUrl,
            ActivityId = Ref.ActivityId,
            Locale = Ref.Locale,
            User = Ref.User
        });

        return res;
    }

    public async Task<MessageActivity> Send(string text)
    {
        var activity = new MessageActivity(text)
        {
            From = Ref.Bot,
            Recipient = Ref.User,
            Conversation = Ref.Conversation
        };

        return await Send(activity);
    }

    public async Task<MessageActivity> Send(Cards.Card card)
    {
        var activity = new MessageActivity()
        {
            From = Ref.Bot,
            Recipient = Ref.User,
            Conversation = Ref.Conversation
        };

        activity = activity.AddAttachment(card);
        return await Send(activity);
    }

    public async Task<TypingActivity> Typing()
    {
        var activity = new TypingActivity()
        {
            From = Ref.Bot,
            Recipient = Ref.User,
            Conversation = Ref.Conversation
        };

        return await Send(activity);
    }

    public IContext<IActivity> ToActivityType()
    {
        return ToActivityType<IActivity>();
    }

    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : IActivity
    {
        return new Context<TToActivity>()
        {
            Sender = Sender,
            AppId = AppId,
            Log = Log,
            Api = Api,
            Activity = (TToActivity)Activity.ToType(typeof(TToActivity), null),
            Ref = Ref,
            UserGraph = UserGraph,
            IsSignedIn = IsSignedIn,
            Extra = Extra,
            ActivitySentEvent = ActivitySentEvent
        };
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}