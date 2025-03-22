using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class CommandResultAttribute() : ActivityAttribute(ActivityType.CommandResult, typeof(CommandResultActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnCommandResult(Func<IContext<CommandResultActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnCommandResult(Func<IContext<CommandResultActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<CommandResultActivity>()),
            Select = activity =>
            {
                if (activity is CommandResultActivity commandResult)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}