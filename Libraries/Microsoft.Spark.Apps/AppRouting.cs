using System.Reflection;

using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial class App
{
    protected List<ActivityHandler> Handlers { get; set; } = [];
    internal readonly Router Router;

    protected static List<ActivityHandler> GetActivityHandlers()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var handlers = new List<ActivityHandler>();

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

                    handlers.Add(new()
                    {
                        Method = method,
                        Attribute = attribute
                    });
                }
            }
        }

        return handlers;
    }

    protected class ActivityHandler
    {
        public required MethodInfo Method { get; set; }
        public required ActivityAttribute Attribute { get; set; }
    }
}