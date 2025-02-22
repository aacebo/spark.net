using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

public interface IRoute
{
    public string? Name { get; }
    public Func<IActivity, bool> Select { get; }
    public Func<IContext<IActivity>, Task> Handler { get; }
}

public class Route : IRoute
{
    public string? Name { get; set; }
    public required Func<IActivity, bool> Select { get; set; }
    public required Func<IContext<IActivity>, Task> Handler { get; set; }
}