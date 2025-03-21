using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class CommandAttribute() : ActivityAttribute(ActivityType.Command, typeof(CommandActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnCommand(Func<IContext<CommandActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnCommand(Func<IContext<CommandActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<CommandActivity>()),
            Selector = activity =>
            {
                if (activity is CommandActivity command)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}