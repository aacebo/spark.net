using System.Reflection;

using Microsoft.Spark.Apps.Routing;

namespace Microsoft.Spark.Apps;

public partial interface IApp : IRoutingModule;

public partial class App : RoutingModule
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

                    Router.Register(new AttributeRoute()
                    {
                        Attr = attribute,
                        Method = method
                    });
                }
            }
        }
    }
}