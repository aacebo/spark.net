using System.Reflection;

using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public ILogger Logger { get; }
    public ApiClient Api { get; }
    public IHttpClient Client { get; }
    public IHttpCredentials? Credentials { get; }
    public IToken? BotToken { get; }
    public IToken? GraphToken { get; }

    public Task Start();
}

public partial class App : IApp
{
    public static IAppBuilder Builder(IAppOptions? options = null) => new AppBuilder(options);

    public ILogger Logger { get; }
    public ApiClient Api { get; }
    public IHttpClient Client { get; }
    public IHttpCredentials? Credentials { get; }
    public IToken? BotToken { get; internal set; }
    public IToken? GraphToken { get; internal set; }

    internal IContainer Container { get; set; }

    public App(IAppOptions? options = null)
    {
        Logger = options?.Logger ?? new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        Client = options?.Client ?? options?.ClientFactory?.CreateClient() ?? new Common.Http.HttpClient();
        Credentials = options?.Credentials;
        Api = new ApiClient(Client);
        Plugins = options?.Plugins ?? [];
        ErrorEvent = (_, args) => OnErrorEvent(args);
        StartEvent = (_, args) => OnStartEvent(args);
        ActivityEvent = (_, plugin, args) => OnActivityEvent(plugin, args);

        Container = new Container();
        Container.Register(Logger);
        Container.Register(Client);
        Container.Register(Api);

        if (Credentials != null)
            Container.Register(Credentials);

        RegisterAttributeRoutes();
    }

    public async Task Start()
    {
        try
        {
            foreach (var plugin in Plugins)
            {
                Inject(plugin);
            }

            if (Credentials != null)
            {
                var botToken = await Api.Bot.Token.GetAsync(Credentials);
                var graphToken = await Api.Bot.Token.GetGraphAsync(Credentials);

                BotToken = new JsonWebToken(botToken.AccessToken);
                GraphToken = new JsonWebToken(graphToken.AccessToken);
            }

            foreach (var plugin in Plugins)
            {
                await plugin.OnInit(this);
            }

            foreach (var plugin in Plugins)
            {
                await plugin.OnStart(this, new()
                {
                    Logger = Logger
                });
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