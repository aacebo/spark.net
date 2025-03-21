using System.Text.RegularExpressions;

using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageAttribute() : ActivityAttribute<MessageActivity>(ActivityType.Message.Value)
{
}

public partial interface IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<MessageActivity>, Task> handler);
    public IAppRouting OnMessage(string pattern, Func<IContext<MessageActivity>, Task> handler);
    public IAppRouting OnMessage(Regex pattern, Func<IContext<MessageActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<MessageActivity>, Task> handler)
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

    public IAppRouting OnMessage(string pattern, Func<IContext<MessageActivity>, Task> handler)
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

    public IAppRouting OnMessage(Regex regex, Func<IContext<MessageActivity>, Task> handler)
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