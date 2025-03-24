using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

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
    /// any extra data
    /// </summary>
    public IDictionary<string, object> Extra { get; set; }

    /// <summary>
    /// send an activity to the conversation
    /// </summary>
    /// <param name="activity">activity activity to send</param>
    public Task<T> Send<T>(T activity) where T : Activity;

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
    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : IActivity;
}

public partial class Context<TActivity> : IContext<TActivity> where TActivity : IActivity
{
    public string AppId { get; set; }
    public ILogger Log { get; set; }
    public ApiClient Api { get; set; }
    public TActivity Activity { get; set; }
    public ConversationReference Ref { get; set; }
    public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();

    protected ISender Sender { get; }

    public Context(ISender sender, string appId, ILogger log, ApiClient api, TActivity activity, ConversationReference reference)
    {
        Sender = sender;
        AppId = appId;
        Log = log;
        Api = api;
        Activity = activity;
        Ref = reference;
    }

    public Context(Context<TActivity> context)
    {
        AppId = context.AppId;
        Log = context.Log;
        Api = context.Api;
        Activity = context.Activity;
        Ref = context.Ref;
        Extra = context.Extra;
        Sender = context.Sender;
    }

    public async Task<T> Send<T>(T activity) where T : Activity
    {
        var res = await Sender.Send(activity, Ref);
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

        var res = await Sender.Send(activity, Ref);
        return res;
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
        var res = await Sender.Send(activity, Ref);
        return res;
    }

    public async Task<TypingActivity> Typing()
    {
        var activity = new TypingActivity()
        {
            From = Ref.Bot,
            Recipient = Ref.User,
            Conversation = Ref.Conversation
        };

        var res = await Sender.Send(activity, Ref);
        return res;
    }

    public IContext<IActivity> ToActivityType()
    {
        return new Context<IActivity>(Sender, AppId, Log, Api, Activity, Ref);
    }

    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : IActivity
    {
        return new Context<TToActivity>(Sender, AppId, Log, Api, (TToActivity)Activity.ToType(typeof(TToActivity), null), Ref);
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