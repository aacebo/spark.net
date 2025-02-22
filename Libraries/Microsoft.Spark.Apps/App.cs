using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public ILogger Logger { get; }

    public void Start();
}

public partial class App : IApp
{
    public ILogger Logger { get; }

    protected IHttpClient Http { get; set; }
    protected IHttpCredentials? Credentials { get; set; }
    protected IList<IPlugin> Plugins { get; set; }
    protected List<ActivityHandler> Handlers { get; set; } = [];
    protected event EventHandler<Events.ErrorEventArgs> Error;

    internal readonly Router Router;

    public App(IAppOptions? options = null)
    {
        Logger = options?.Logger ?? new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        Http = options?.Http ?? options?.HttpFactory?.CreateClient() ?? new Common.Http.HttpClient();
        Credentials = options?.Credentials;
        Plugins = options?.Plugins ?? [];
        Router = new Router();
        Error = OnError;
        Handlers = GetActivityHandlers();

        foreach (var handler in Handlers)
        {
            Router.Register(
                handler.Attribute.Name,
                delegate (IActivityContext<IActivity> context)
                {
                    return Task.Run(() => { });
                }
            );
        }
    }

    public void Start()
    {

    }

    protected void OnError(object? sender, Events.ErrorEventArgs e)
    {
        
    }

    public static IAppBuilder Builder(IAppOptions? options = null)
    {
        return new AppBuilder(options);
    }

    protected static List<ActivityHandler> GetActivityHandlers()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var handlers = new List<ActivityHandler>();

        foreach (Type type in assembly.GetTypes())
        {
            var methods = type.GetMethods();

            foreach (MethodInfo method in methods)
            {
                var attrs = method.GetCustomAttributes(typeof(OnAttribute), true);

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
                    var attribute = (OnAttribute)attr;

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
        public required OnAttribute Attribute { get; set; }
    }
}