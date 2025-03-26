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
        Send = 8,
        Context = AppId | Activity | Ref | Send,
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ActivityAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class LoggerAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class SendAttribute() : Attribute
    {

    }

    /// <summary>
    /// an object that can send activities
    /// </summary>
    /// <param name="context">the parent context</param>
    public class Send(IContext<IActivity> context)
    {
        public Func<IActivity, Task<IActivity>> Activity { get; set; } = context.Send;
        public Func<string, Task<MessageActivity>> Text { get; set; } = context.Send;
        public Func<Cards.Card, Task<MessageActivity>> Card { get; set; } = context.Send;
        public Func<Task<TypingActivity>> Typing { get; set; } = context.Typing;
    }
}