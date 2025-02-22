using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

internal class Router
{
    internal int Length { get => _routes.Count(); }

    protected readonly List<Route> _routes = [];

    internal List<Route> Select(IActivity activity)
    {
        return _routes
            .Where(route => route.Select.Invoke(activity))
            .ToList();
    }

    internal Router Register(Route route)
    {
        _routes.Add(route);
        return this;
    }

    internal Router Register(Func<IContext<IActivity>, Task> handler)
    {
        return Register(new Route()
        {
            Select = _ => true,
            Handler = handler
        });
    }

    internal Router Register(string name, Func<IContext<IActivity>, Task> handler)
    {
        return Register(new Route()
        {
            Name = name,
            Handler = handler,
            Select = activity =>
            {
                if (name == "activity") return true;
                if (name == activity.Type) return true;

                return false;
            }
        });
    }

    internal class Route
    {
        public string? Name { get; set; }
        public required Func<IActivity, bool> Select { get; set; }
        public required Func<IContext<IActivity>, Task> Handler { get; set; }
    }
}