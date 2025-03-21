using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public partial class ActivityAttribute(string name = "activity", Type? type = null) : Attribute
{
    public readonly string Name = name;
    public readonly Type Type = type ?? typeof(Activity);
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public partial class ActivityAttribute<TActivity>(string name = "activity") : Attribute where TActivity : Activity
{
    public readonly string Name = name;
    public readonly Type Type = typeof(TActivity);
}

public partial interface IAppRouting
{
    public IAppRouting OnActivity(Func<IContext<Activity>, Task> handler);
    public IAppRouting OnActivity(Func<Activity, bool> select, Func<IContext<Activity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    protected IRouter Router { get; } = new Router();

    public IAppRouting OnActivity(Func<IContext<Activity>, Task> handler)
    {
        Router.Register(handler);
        return this;
    }

    public IAppRouting OnActivity(Func<Activity, bool> select, Func<IContext<Activity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Select = select,
            Handler = handler
        });

        return this;
    }
}