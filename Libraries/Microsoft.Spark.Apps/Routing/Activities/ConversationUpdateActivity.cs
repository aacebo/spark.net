using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ConversationUpdateAttribute() : ActivityAttribute<ConversationUpdateActivity>(ActivityType.ConversationUpdate.Value)
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnConversationUpdate(Func<IContext<ConversationUpdateActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnConversationUpdate(Func<IContext<ConversationUpdateActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<ConversationUpdateActivity>()),
            Select = activity =>
            {
                if (activity is ConversationUpdateActivity conversationUpdate)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}