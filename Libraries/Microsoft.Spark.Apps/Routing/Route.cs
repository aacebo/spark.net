using System.Reflection;

using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

public interface IRoute
{
    public bool Select(IActivity activity);
    public Task Invoke(IContext<IActivity> context);
}

public class Route : IRoute
{
    public string? Name { get; set; }
    public required Func<IActivity, bool> Selector { get; set; }
    public required Func<IContext<IActivity>, Task> Handler { get; set; }

    public bool Select(IActivity activity) => Selector(activity);
    public Task Invoke(IContext<IActivity> context) => Handler(context);
}

public class AttributeRoute : IRoute
{
    public required ActivityAttribute Attr { get; set; }
    public required MethodInfo Method { get; set; }

    public bool Select(IActivity activity) => Attr.Select(activity);
    public async Task Invoke(IContext<IActivity> context)
    {
        object? res = null;

        if (Attr.Name == null)
        {
            res = Method.Invoke(null, [context]);
        }
        else
        {
            res = Method.Invoke(null, [Attr.Coerce(context)]);
        }

        if (res is Task task)
            await task;
    }
}