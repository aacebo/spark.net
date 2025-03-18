using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial interface IApp : IAppRouting;

public partial class App : AppRouting
{
    protected void RegisterAttributeRoutes()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

        foreach (Type type in assembly.GetTypes())
        {
            var methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                var attrs = method.GetCustomAttributes(typeof(ActivityAttribute), true);

                if (attrs.Length == 0) continue;

                var param = method.GetParameters().FirstOrDefault();

                if (param == null)
                {
                    throw new ArgumentException("Activity handlers must have 1 parameter of type `ActivityContext`");
                }

                var generic = param.ParameterType.GenericTypeArguments.FirstOrDefault();

                if (generic == null)
                {
                    throw new ArgumentException("Activity handlers must have 1 parameter of type `ActivityContext`");
                }

                foreach (object attr in attrs)
                {
                    var attribute = (ActivityAttribute)attr;

                    if (!attribute.Type.IsAssignableTo(generic))
                    {
                        throw new ArgumentException($"'{generic.Name}' is not assignable to '{attribute.Type.Name}'");
                    }

                    Router.Register(attribute.Name, async (IContext<Activity> context) =>
                    {
                        if (!attribute.Type.IsAssignableFrom(context.Activity.GetType())) return;

                        context.Activity = context.Activity.ToActivity();
                        object? res = null;
                        res = method.Invoke(null, [context]);

                        // if (attribute.Name == "activity")
                        // {
                        //     res = method.Invoke(null, [context]);
                        // }
                        // else if (attribute.Name == "message")
                        // {
                        //     res = method.Invoke(null, [context.ToActivityType<MessageActivity>()]);
                        // }
                        // else if (attribute.Name == "messageUpdate")
                        // {
                        //     res = method.Invoke(null, [context.ToActivityType<MessageUpdateActivity>()]);
                        // }

                        if (res is Task task)
                            await task;
                    });
                }
            }
        }
    }
}