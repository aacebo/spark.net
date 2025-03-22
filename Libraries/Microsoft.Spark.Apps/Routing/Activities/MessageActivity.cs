using System.Text.RegularExpressions;

using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageAttribute() : ActivityAttribute(ActivityType.Message, typeof(MessageActivity))
{
}

public partial interface IRoutingModule
{
    public IRoutingModule OnMessage(Func<IContext<MessageActivity>, Task> handler);
    public IRoutingModule OnMessage(string pattern, Func<IContext<MessageActivity>, Task> handler);
    public IRoutingModule OnMessage(Regex pattern, Func<IContext<MessageActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnMessage(Func<IContext<MessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageActivity>()),
            Select = activity =>
            {
                if (activity is MessageActivity message)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }

    public IRoutingModule OnMessage(string pattern, Func<IContext<MessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageActivity>()),
            Select = activity =>
            {
                if (activity is MessageActivity message)
                {
                    return new Regex(pattern).IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }

    public IRoutingModule OnMessage(Regex regex, Func<IContext<MessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<MessageActivity>()),
            Select = activity =>
            {
                if (activity is MessageActivity message)
                {
                    return regex.IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }
}