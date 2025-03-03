using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

public interface IRouter
{
    public int Length { get; }

    public IList<IRoute> Select(IActivity activity);
    public IRouter Register(IRoute route);
    public IRouter Register(Func<IContext<IActivity>, Task> handler);
    public IRouter Register(string name, Func<IContext<IActivity>, Task> handler);
}

public class Router : IRouter
{
    public int Length { get => _routes.Count; }

    protected readonly List<IRoute> _routes = [];

    public IList<IRoute> Select(IActivity activity)
    {
        return _routes
            .Where(route => route.Select(activity))
            .ToList();
    }

    public IRouter Register(IRoute route)
    {
        _routes.Add(route);
        return this;
    }

    public IRouter Register(Func<IContext<IActivity>, Task> handler)
    {
        return Register(new Route()
        {
            Select = _ => true,
            Handler = handler
        });
    }

    public IRouter Register(string name, Func<IContext<IActivity>, Task> handler)
    {
        return Register(new Route()
        {
            Name = name,
            Handler = handler,
            Select = (activity) =>
            {
                if (name == null || name == "activity") return true;
                if (name == activity.Type) return true;

                return false;
            }
        });
    }
}