using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class EndOfConversationAttribute() : ActivityAttribute(ActivityType.EndOfConversation, typeof(EndOfConversationActivity))
{
    public override object Coerce(IContext<IActivity> context) => context.ToActivityType<EndOfConversationActivity>();
}

public partial interface IRoutingModule
{
    public IRoutingModule OnEndOfConversation(Func<IContext<EndOfConversationActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnEndOfConversation(Func<IContext<EndOfConversationActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<EndOfConversationActivity>()),
            Selector = activity =>
            {
                if (activity is EndOfConversationActivity endOfConversation)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}