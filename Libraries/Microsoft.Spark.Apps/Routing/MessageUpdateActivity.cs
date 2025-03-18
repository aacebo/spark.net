using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageUpdateAttribute() : ActivityAttribute("messageUpdate", typeof(IMessageUpdateActivity))
{
}

public partial interface IAppRouting
{
    public IAppRouting OnMessageUpdate(Func<IContext<IMessageUpdateActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    public IAppRouting OnMessageUpdate(Func<IContext<IMessageUpdateActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<IMessageUpdateActivity>()),
            Select = activity =>
            {
                if (activity is IMessageUpdateActivity messageUpdate)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}