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
    public bool Validate()
    {
        foreach (var param in Method.GetParameters())
        {
            var logger = param.GetCustomAttribute<IContext.LoggerAttribute>();
            var activity = param.GetCustomAttribute<IContext.ActivityAttribute>();

            var generic = param.ParameterType.GenericTypeArguments.FirstOrDefault();
            var isContext = generic?.IsAssignableTo(Attr.Type) ?? false;

            if (logger == null && activity == null && !isContext)
                return false;
        }

        return true;
    }

    public async Task Invoke(IContext<IActivity> context)
    {
        object? res = null;

        var args = Method.GetParameters().Select(param =>
        {
            var logger = param.GetCustomAttribute<IContext.LoggerAttribute>();
            var activity = param.GetCustomAttribute<IContext.ActivityAttribute>();

            if (logger != null) return context.Log;
            if (activity != null) return context.Activity.ToType(param.ParameterType, null);
            return Attr.Coerce(context);
        });

        if (Attr.Name == null)
        {
            res = Method.Invoke(null, args?.ToArray());
        }
        else
        {
            res = Method.Invoke(null, args?.ToArray());
        }

        if (res is Task task)
            await task;
    }
}