using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageUpdateAttribute() : ActivityAttribute("messageUpdate", typeof(MessageUpdateActivity))
{
}

public partial interface IAppRouting
{
    public IAppRouting OnMessageUpdate(Func<IContext<MessageUpdateActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    public IAppRouting OnMessageUpdate(Func<IContext<MessageUpdateActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageUpdateActivity>()),
            Select = activity =>
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