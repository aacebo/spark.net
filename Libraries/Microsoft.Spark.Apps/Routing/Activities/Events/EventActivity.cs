using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class EventAttribute() : ActivityAttribute(ActivityType.Event, typeof(EventActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnEvent(Func<IContext<EventActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnEvent(Func<IContext<EventActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<EventActivity>()),
            Selector = activity =>
            {
                if (activity is EventActivity @event)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}