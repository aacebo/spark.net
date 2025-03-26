using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

internal delegate Task<object?> NextHandler();
internal delegate Task ActivitySentHandler(ISender plugin, Events.ActivitySentEventArgs args);

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
    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity, out IContext.Client client);

    /// <summary>
    /// destruct the context
    /// </summary>
    /// <param name="appId">the apps id</param>
    /// <param name="log">the ILogger instance</param>
    /// <param name="api">the api client</param>
    /// <param name="activity">the inbound activity</param>
    /// <param name="reference">the inbound conversation reference</param>
    /// <param name="send">the methods to send activities</param>
    public void Deconstruct(out string appId, out ILogger log, out ApiClient api, out TActivity activity, out ConversationReference reference, out IContext.Client client);

    /// <summary>
    /// called to continue the chain of route handlers,
    /// if not called no other handlers in the sequence will be executed
    /// </summary>
    public Task<object?> Next();

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

    internal NextHandler OnNext { get; set; } = () => Task.FromResult<object?>(null);
    internal ActivitySentHandler OnActivitySent { get; set; } = (_, _) => Task.Run(() => { });

    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity)
    {
        log = Log;
        api = Api;
        activity = Activity;
    }

    public void Deconstruct(out ILogger log, out ApiClient api, out TActivity activity, out IContext.Client client)
    {
        log = Log;
        api = Api;
        activity = Activity;
        client = new IContext.Client(ToActivityType());
    }

    public void Deconstruct(out string appId, out ILogger log, out ApiClient api, out TActivity activity, out ConversationReference reference, out IContext.Client client)
    {
        appId = AppId;
        log = Log;
        api = Api;
        activity = Activity;
        reference = Ref;
        client = new IContext.Client(ToActivityType());
    }

    public Task<object?> Next() => OnNext();
    public IContext<IActivity> ToActivityType() => ToActivityType<IActivity>();
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
            OnNext = OnNext,
            OnActivitySent = OnActivitySent
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