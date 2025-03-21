using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class InvokeAttribute() : ActivityAttribute(ActivityType.Invoke.Value, typeof(InvokeActivity))
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
            Select = activity =>
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