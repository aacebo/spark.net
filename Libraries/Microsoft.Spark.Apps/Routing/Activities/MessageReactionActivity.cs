using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageReactionAttribute() : ActivityAttribute<MessageReactionActivity>(ActivityType.MessageReaction.Value)
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnMessageReaction(Func<IContext<MessageReactionActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnMessageReaction(Func<IContext<MessageReactionActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageReactionActivity>()),
            Select = activity =>
            {
                if (activity is MessageReactionActivity messageReaction)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}