using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps.Routing;

public interface IContext<TActivity> where TActivity : Activity
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
    /// convert the context to that of another activity type
    /// </summary>
    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : TActivity;
}

public class Context<TActivity>(ISender sender) : IContext<TActivity> where TActivity : Activity
{
    public required string AppId { get; set; }
    public required ILogger Log { get; set; }
    public required ApiClient Api { get; set; }
    public required TActivity Activity { get; set; }
    public required ConversationReference Ref { get; set; }
    public IDictionary<string, object> Extra { get; set; } = new Dictionary<string, object>();

    protected ISender Sender { get; } = sender;

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

    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : TActivity
    {
        return new Context<TToActivity>(Sender)
        {
            AppId = AppId,
            Log = Log,
            Api = Api,
            Ref = Ref,
            Extra = new Dictionary<string, object>(),
            Activity = (TToActivity)Activity
        };
    }
}