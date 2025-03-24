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
    public ValidationResult Validate()
    {
        var result = new ValidationResult();

        foreach (var param in Method.GetParameters())
        {
            var logger = param.GetCustomAttribute<IContext.LoggerAttribute>();
            var activity = param.GetCustomAttribute<IContext.ActivityAttribute>();

            var generic = param.ParameterType.GenericTypeArguments.FirstOrDefault();
            var isContext = generic?.IsAssignableTo(Attr.Type) ?? false;

            if (logger == null && activity == null && !isContext)
                result.AddError(param.Name ?? "??", "type must be `IContext<TActivity>` or an `IContext` property attribute");
        }

        return result;
    }

    public async Task Invoke(IContext<IActivity> context)
    {
        context.Log = context.Log.Child(Method.Name);

        var args = Method.GetParameters().Select(param =>
        {
            var logger = param.GetCustomAttribute<IContext.LoggerAttribute>();
            var activity = param.GetCustomAttribute<IContext.ActivityAttribute>();

            if (logger != null) return context.Log;
            if (activity != null) return context.Activity.ToType(param.ParameterType, null);
            return Attr.Coerce(context);
        });

        if (Attr.Log.HasFlag(IContext.Property.Context))
        {
            context.Log.Debug(context);
        }
        else
        {
            if (Attr.Log.HasFlag(IContext.Property.AppId))
                context.Log.Debug(context.AppId);

            if (Attr.Log.HasFlag(IContext.Property.Activity))
                context.Log.Debug(context.Activity);
        }

        var res = Method.Invoke(null, args?.ToArray());

        if (res is Task task)
            await task;
    }

    public class ValidationResult
    {
        /// <summary>
        /// the errors that were found
        /// </summary>
        public IList<ParameterError> Errors { get; set; } = [];

        /// <summary>
        /// is the result valid
        /// </summary>
        public bool Valid => Errors.Count == 0;

        /// <summary>
        /// combine all the errors into
        /// one message string
        /// </summary>
        public override string ToString() => string.Join('\n', Errors.Select(err => $"{err.Name} => {err.Message}"));

        /// <summary>
        /// add a parameter error to the result
        /// </summary>
        public void AddError(string name, string message)
        {
            Errors.Add(new(name, message));
        }

        public record ParameterError(string Name, string Message);
    }
}