using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class TypingAttribute() : ActivityAttribute(ActivityType.Typing.Value, typeof(TypingActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnTyping(Func<IContext<TypingActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnTyping(Func<IContext<TypingActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<TypingActivity>()),
            Select = activity =>
            {
                if (activity is TypingActivity typing)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}