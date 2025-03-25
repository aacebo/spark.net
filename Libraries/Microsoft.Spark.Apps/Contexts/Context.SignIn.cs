using System.Text.Json;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps;

public partial interface IContext<TActivity>
{
    /// <summary>
    /// is the activity sender signed in?
    /// </summary>
    public bool IsSignedIn { get; set; }

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

public partial class Context<TActivity> : IContext<TActivity>
{
    public bool IsSignedIn { get; set; } = false;

    public async Task<string?> SignIn(string connectionName = "graph", string oauthCardText = "Please Sign In...", string signInButtonText = "Sign In")
    {
        var reference = Ref.Copy();

        try
        {
            var res = await Api.Users.Token.GetAsync(new()
            {
                UserId = Activity.From.Id,
                ChannelId = Activity.ChannelId,
                ConnectionName = connectionName,
            });

            return res.Token;
        }
        catch { }

        // create new 1:1 conversation with user to do SSO
        // because groupchats don't support it.
        if (Activity.Conversation.IsGroup == true)
        {
            var res = await Api.Conversations.CreateAsync(new()
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
        var resource = await Api.Bots.SignIn.GetResourceAsync(new() { State = state });
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
                new(Spark.Api.Cards.ActionType.SignIn)
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
        await Api.Users.Token.SignOutAsync(new()
        {
            ChannelId = Ref.ChannelId,
            UserId = Activity.From.Id,
            ConnectionName = connectionName,
        });
    }
}