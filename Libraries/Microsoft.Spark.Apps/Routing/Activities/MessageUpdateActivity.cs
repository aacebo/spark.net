using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageUpdateAttribute() : ActivityAttribute(ActivityType.MessageUpdate, typeof(MessageUpdateActivity))
{
    public override object Coerce(IContext<IActivity> context) => context.ToActivityType<MessageUpdateActivity>();
}

public partial interface IRoutingModule
{
    public IRoutingModule OnMessageUpdate(Func<IContext<MessageUpdateActivity>, Task<object?>> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnMessageUpdate(Func<IContext<MessageUpdateActivity>, Task<object?>> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageUpdateActivity>()),
            Selector = activity =>
            {
                if (activity is MessageUpdateActivity messageUpdate)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}