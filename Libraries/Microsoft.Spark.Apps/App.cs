using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Routing;
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
                delegate (IContext<IActivity> context)
                {
                    return Task.Run(() => { });
                }
            );
        }
    }

    public static IAppBuilder Builder(IAppOptions? options = null)
    {
        return new AppBuilder(options);
    }

    public void Start()
    {

    }
}