using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public partial class ActivityAttribute(ActivityType? name = null, Type? type = null) : Attribute
{
    public readonly ActivityType? Name = name;
    public readonly Type Type = type ?? typeof(Activity);
}

public partial interface IRoutingModule
{
    public IRoutingModule OnActivity(Func<IContext<Activity>, Task> handler);
    public IRoutingModule OnActivity(ActivityType type, Func<IContext<Activity>, Task> handler);
    public IRoutingModule OnActivity<TActivity>(Func<IContext<TActivity>, Task> handler) where TActivity : Activity;
    public IRoutingModule OnActivity(Func<Activity, bool> select, Func<IContext<Activity>, Task> handler);
}

public partial class RoutingModule : IRoutingModule
{
    protected IRouter Router { get; } = new Router();

    public IRoutingModule OnActivity(Func<IContext<Activity>, Task> handler)
    {
        Router.Register(handler);
        return this;
    }

    public IRoutingModule OnActivity(ActivityType type, Func<IContext<Activity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Handler = handler,
            Selector = (activity) => activity.Type.Equals(type),
        });

        return this;
    }

    public IRoutingModule OnActivity<TActivity>(Func<IContext<TActivity>, Task> handler) where TActivity : Activity
    {
        Router.Register(new Route()
        {
            Handler = (context) => handler(context.ToActivityType<TActivity>()),
            Selector = (activity) => activity.GetType() == typeof(TActivity),
        });

        return this;
    }

    public IRoutingModule OnActivity(Func<Activity, bool> select, Func<IContext<Activity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Selector = select,
            Handler = handler
        });

        return this;
    }
}