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

                foreach (object attr in attrs)
                {
                    var attribute = (ActivityAttribute)attr;
                    var route = new AttributeRoute()
                    {
                        Attr = attribute,
                        Method = method
                    };

                    if (!route.Validate()) throw new InvalidOperationException("invalid activity handler");
                    Router.Register(route);
                }
            }
        }
    }
}