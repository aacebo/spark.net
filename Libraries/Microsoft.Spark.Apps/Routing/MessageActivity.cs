using System.Text.RegularExpressions;

using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageAttribute() : ActivityAttribute("message", typeof(IMessageActivity))
{
}

public partial interface IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<IMessageActivity>, Task> handler);
    public IAppRouting OnMessage(string pattern, Func<IContext<IMessageActivity>, Task> handler);
    public IAppRouting OnMessage(Regex pattern, Func<IContext<IMessageActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<IMessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<IMessageActivity>()),
            Select = activity =>
            {
                if (activity is IMessageActivity message)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }

    public IAppRouting OnMessage(string pattern, Func<IContext<IMessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<IMessageActivity>()),
            Select = activity =>
            {
                if (activity is IMessageActivity message)
                {
                    return new Regex(pattern).IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }

    public IAppRouting OnMessage(Regex regex, Func<IContext<IMessageActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<IMessageActivity>()),
            Select = activity =>
            {
                if (activity is IMessageActivity message)
                {
                    return regex.IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }
}