using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityAttribute(string? name = null, Type? type = null) : Attribute
{
    public readonly string Name = name ?? "activity";
    public readonly Type Type = type ?? typeof(IActivity);
}

public partial interface IAppRouting
{
    public IAppRouting OnActivity(Func<IContext<IActivity>, Task> handler);
    public IAppRouting OnActivity(Func<IActivity, bool> select, Func<IContext<IActivity>, Task> handler);
}

public partial class AppRouting : IAppRouting
{
    protected IRouter Router { get; } = new Router();

    public IAppRouting OnActivity(Func<IContext<IActivity>, Task> handler)
    {
        Router.Register(handler);
        return this;
    }

    public IAppRouting OnActivity(Func<IActivity, bool> select, Func<IContext<IActivity>, Task> handler)
    {
        Router.Register(new Route()
        {
            Select = select,
            Handler = handler
        });

        return this;
    }
}