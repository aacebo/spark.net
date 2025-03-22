using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class InvokeAttribute() : ActivityAttribute(ActivityType.Invoke, typeof(InvokeActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnInvoke(Func<IContext<InvokeActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnInvoke(Func<IContext<InvokeActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<InvokeActivity>()),
            Selector = activity =>
            {
                if (activity is InvokeActivity invoke)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}