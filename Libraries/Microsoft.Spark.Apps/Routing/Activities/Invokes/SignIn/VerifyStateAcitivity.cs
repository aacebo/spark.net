using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Activities.Invokes;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class VerifyStateAttribute(IContext.Property log = IContext.Property.None) : InvokeAttribute(Api.Activities.Invokes.Name.SignIn.VerifyState, typeof(SignIn.VerifyStateActivity), log)
{
    public override object Coerce(IContext<IActivity> context) => context.ToActivityType<SignIn.VerifyStateActivity>();
}

public partial interface IRoutingModule
{
    public IRoutingModule OnVerifyState(Func<IContext<SignIn.VerifyStateActivity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnVerifyState(Func<IContext<SignIn.VerifyStateActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<SignIn.VerifyStateActivity>()),
            Selector = activity =>
            {
                if (activity is SignIn.VerifyStateActivity tokenExchange)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}