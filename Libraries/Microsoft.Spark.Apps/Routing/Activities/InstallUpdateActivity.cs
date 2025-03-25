using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class InstallUpdateAttribute() : ActivityAttribute(ActivityType.InstallUpdate, typeof(InstallUpdateActivity))
{
    public override object Coerce(IContext<IActivity> context) => context.ToActivityType<InstallUpdateActivity>();
}

public partial interface IRoutingModule
{
    public IRoutingModule OnInstallUpdate(Func<IContext<InstallUpdateActivity>, Task<object?>> handler);
}

public partial class RoutingModule : IRoutingModule
{
    public IRoutingModule OnInstallUpdate(Func<IContext<InstallUpdateActivity>, Task<object?>> handler)
    {
        Router.Register(new Route()
        {
            Handler = context => handler(context.ToActivityType<InstallUpdateActivity>()),
            Selector = activity =>
            {
                if (activity is InstallUpdateActivity installUpdate)
                {
                    return true;
                }

                return false;
            }
        });

        return this;
    }
}