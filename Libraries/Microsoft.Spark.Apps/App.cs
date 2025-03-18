using System.Reflection;

using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public ILogger Logger { get; }

    public Task Start();
}

public partial class App : IApp
{
    public static IAppBuilder Builder(IAppOptions? options = null) => new AppBuilder(options);

    public ILogger Logger { get; }

    protected IHttpClient Http { get; set; }
    protected IHttpCredentials? Credentials { get; set; }

    internal IContainer Container { get; set; }

    public App(IAppOptions? options = null)
    {
        Logger = options?.Logger ?? new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        Http = options?.Http ?? options?.HttpFactory?.CreateClient() ?? new Common.Http.HttpClient();
        Credentials = options?.Credentials;
        Plugins = options?.Plugins ?? [];
        ErrorEvent = (_, args) => OnErrorEvent(args);
        StartEvent = (_, args) => OnStartEvent(args);
        ActivityEvent = (_, plugin, args) => OnActivityEvent(plugin, args);

        Container = new Container();
        Container.Register(Logger);

        RegisterAttributeRoutes();
    }

    public async Task Start()
    {
        try
        {
            foreach (var plugin in Plugins)
            {
                Inject(plugin);
                await plugin.OnInit(this);
            }

            await StartEvent(this, new() { Logger = Logger });
        }
        catch (Exception err)
        {
            await ErrorEvent(this, new()
            {
                Error = err,
                Logger = Logger
            });
        }
    }
}