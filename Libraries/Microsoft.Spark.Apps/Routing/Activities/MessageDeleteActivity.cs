using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageDeleteAttribute() : ActivityAttribute(ActivityType.MessageDelete, typeof(MessageDeleteActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnMessageDelete(Func<IContext<MessageDeleteActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnMessageDelete(Func<IContext<MessageDeleteActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageDeleteActivity>()),
            Selector = activity =>
            {
                if (activity is MessageDeleteActivity messageDelete)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}