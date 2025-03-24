using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Invokes;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class TokenExchangeAttribute(IContext.Property log = IContext.Property.None) : InvokeAttribute(Api.Activities.Invokes.Name.SignIn.TokenExchange, typeof(SignIn.TokenExchangeActivity), log)
{
    public override object Coerce(IContext<IActivity> context) => context.ToActivityType<SignIn.TokenExchangeActivity>();
}

public partial interface IRoutingModule
{
    public IRoutingModule OnTokenExchange(Func<IContext<SignIn.TokenExchangeActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnTokenExchange(Func<IContext<SignIn.TokenExchangeActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<SignIn.TokenExchangeActivity>()),
            Selector = activity =>
            {
                if (activity is SignIn.TokenExchangeActivity tokenExchange)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}