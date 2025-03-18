using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

public interface IRoute
{
    public string? Name { get; }
    public Func<Activity, bool> Select { get; }

    public Task Invoke(IContext<Activity> context);
}

public class Route : IRoute
{
    public string? Name { get; set; }
    public required Func<Activity, bool> Select { get; set; }
    public required Func<IContext<Activity>, Task> Handler { get; set; }

    public Task Invoke(IContext<Activity> context)
    {
        return Handler(context);
    }
}