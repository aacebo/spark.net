using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps;

public partial interface IContext
{
    [Flags]
    public enum Property
    {
        None = 0,
        AppId = 1,
        Activity = 2,
        Ref = 4,
        Context = AppId | Activity | Ref,
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class AppIdAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class LoggerAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ApiAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ActivityAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class RefAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class UserGraphAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ClientAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class IsSignedInAttribute() : Attribute
    {

    }

    /// <summary>
    /// an object that can send activities
    /// </summary>
    /// <param name="context">the parent context</param>
    public class Client(IContext<IActivity> context)
    {
        /// <summary>
        /// send an activity to the conversation
        /// </summary>
        /// <param name="activity">activity activity to send</param>
        public Task<T> Send<T>(T activity) where T : IActivity => context.Send(activity);

        /// <summary>
        /// send a message activity to the conversation
        /// </summary>
        /// <param name="text">the text to send</param>
        public Task<MessageActivity> Send(string text) => context.Send(text);

        /// <summary>
        /// send a message activity with a card attachment
        /// </summary>
        /// <param name="card">the card to send as an attachment</param>
        public Task<MessageActivity> Send(Cards.Card card) => context.Send(card);

        /// <summary>
        /// send a typing activity
        /// </summary>
        public Task<TypingActivity> Typing() => context.Typing();

        /// <summary>
        /// trigger user signin flow for the activity sender
        /// </summary>
        /// <param name="options">option overrides</param>
        /// <returns>the existing user token if found</returns>
        public Task<string?> SignIn(SignInOptions? options = null) => context.SignIn(options);

        /// <summary>
        /// trigger user signin flow for the activity sender
        /// </summary>
        /// <param name="connectionName">the connection name</param>
        public Task SignOut(string? connectionName = null) => context.SignOut(connectionName);
    }
}