using System.Text.RegularExpressions;

using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageAttribute() : ActivityAttribute("message", typeof(IMessageSendActivity))
{
}

public partial interface IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<IMessageSendActivity>, Task> handler);
    public IAppRouting OnMessage(string pattern, Func<IContext<IMessageSendActivity>, Task> handler);
    public IAppRouting OnMessage(Regex pattern, Func<IContext<IMessageSendActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    public IAppRouting OnMessage(Func<IContext<IMessageSendActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler((IContext<IMessageSendActivity>)context),
            Select = activity =>
            {
                if (activity is IMessageSendActivity message)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }

    public IAppRouting OnMessage(string pattern, Func<IContext<IMessageSendActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler((IContext<IMessageSendActivity>)context),
            Select = activity =>
            {
                if (activity is IMessageSendActivity message)
                {
                    return new Regex(pattern).IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }

    public IAppRouting OnMessage(Regex regex, Func<IContext<IMessageSendActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler((IContext<IMessageSendActivity>)context),
            Select = activity =>
            {
                if (activity is IMessageSendActivity message)
                {
                    return regex.IsMatch(message.Text);
                }

                return false;
            }
        });

        return this;
    }
}