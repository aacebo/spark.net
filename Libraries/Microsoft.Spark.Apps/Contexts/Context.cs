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
    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : IActivity;

    /// <summary>
    /// trigger user signin flow for the activity sender
    /// </summary>
    /// <param name="connectionName">the connection name</param>
    /// <param name="oauthCardText">the oauth card text</param>
    /// <param name="signInButtonText">the signin button text</param>
    /// <returns>the existing user token if found</returns>
    public Task<string?> SignIn(
        string connectionName = "graph",
        string oauthCardText = "Please Sign In...",
        string signInButtonText = "Sign In"
    );

    /// <summary>
    /// trigger user signin flow for the activity sender
    /// </summary>
    /// <param name="connectionName">the connection name</param>
    public Task SignOut(string connectionName = "graph");
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

    public async Task<T> Send<T>(T activity) where T : IActivity
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

    public async Task<string?> SignIn(string connectionName = "graph", string oauthCardText = "Please Sign In...", string signInButtonText = "Sign In")
    {
        var reference = Ref.Copy();

        try
        {
            var res = await Api.Users.Token.GetAsync(new UserTokenClient.GetTokenRequest()
            {
                UserId = Activity.From.Id,
                ChannelId = Activity.ChannelId,
                ConnectionName = connectionName
            });

            return res.AccessToken;
        }
        catch { }

        // create new 1:1 conversation with user to do SSO
        // because groupchats don't support it.
        if (Activity.Conversation.IsGroup == true)
        {
            var res = await Api.Conversations.CreateAsync(new ConversationClient.CreateRequest()
            {
                TenantId = Ref.Conversation.TenantId,
                IsGroup = false,
                Bot = Ref.Bot,
                Members = [Activity.From]
            });

            await Send(oauthCardText);
            reference.Conversation.Id = res.Id;
            reference.Conversation.IsGroup = false;
        }

        var tokenExchangeState = new Api.TokenExchange.State()
        {
            ConnectionName = connectionName,
            Conversation = reference,
            RelatesTo = Activity.RelatesTo,
            MsAppId = AppId
        };

        var state = Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(tokenExchangeState));
        var resource = await Api.Bots.SignIn.GetResourceAsync(new BotSignInClient.GetResourceRequest()
        {
            State = state
        });

        var activity = new MessageActivity();
        activity.InputHint = InputHint.AcceptingInput;
        activity.Recipient = Activity.From;
        activity.Conversation = reference.Conversation;
        activity.AddAttachment(new Api.Cards.OAuthCard()
        {
            Text = oauthCardText,
            ConnectionName = connectionName,
            TokenExchangeResource = resource.TokenExchangeResource,
            TokenPostResource = resource.TokenPostResource,
            Buttons = [
                new Api.Cards.Action(Spark.Api.Cards.ActionType.SignIn)
                {
                    Title = signInButtonText,
                    Value = resource.SignInLink
                }
            ]
        });

        Log.Debug(activity);
        await Send(activity);
        return null;
    }

    public async Task SignOut(string connectionName = "graph")
    {
        await Api.Users.Token.SignOutAsync(new UserTokenClient.SignOutRequest()
        {
            ChannelId = Ref.ChannelId,
            UserId = Activity.From.Id,
            ConnectionName = connectionName,
        });
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