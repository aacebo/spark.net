using System.Reflection;

using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;
using Microsoft.Spark.Common.Storage;

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

    /// <summary>
    /// the apps id
    /// </summary>
    public string? Id => BotToken?.AppId ?? GraphToken?.AppId;

    /// <summary>
    /// the apps name
    /// </summary>
    public string? Name => BotToken?.AppDisplayName ?? GraphToken?.AppDisplayName;

    public ILogger Logger { get; }
    public IStorage<string, object> Storage { get; }
    public ApiClient Api { get; }
    public IHttpClient Client { get; }
    public IHttpCredentials? Credentials { get; }
    public IToken? BotToken { get; internal set; }
    public IToken? GraphToken { get; internal set; }

    internal IContainer Container { get; set; }
    internal string UserAgent
    {
        get
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            version ??= "0.0.0";
            return $"spark.net[apps]/{version}";
        }
    }

    public App(IAppOptions? options = null)
    {
        Logger = options?.Logger ?? new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        Storage = options?.Storage ?? new LocalStorage<object>();
        Client = options?.Client ?? options?.ClientFactory?.CreateClient() ?? new Common.Http.HttpClient();
        Client.Options.TokenFactory = () => BotToken;
        Client.Options.AddUserAgent(UserAgent);
        Credentials = options?.Credentials;
        Api = new ApiClient("https://smba.trafficmanager.net/teams", Client);
        Plugins = options?.Plugins ?? [];
        ErrorEvent = (_, args) => OnErrorEvent(args);
        StartEvent = (_, args) => OnStartEvent(args);
        ActivityEvent = (_, plugin, args) => OnActivityEvent(plugin, args);

        Container = new Container();
        Container.Register(Logger);
        Container.Register(Storage);
        Container.Register(Client);
        Container.Register(Api);
        Container.Register<IHttpCredentials>(new FactoryProvider(() => Credentials));
        Container.Register("AppId", new FactoryProvider(() => Id));
        Container.Register("AppName", new FactoryProvider(() => Name));
        Container.Register("BotToken", new FactoryProvider(() => BotToken));
        Container.Register("GraphToken", new FactoryProvider(() => GraphToken));

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
                var botToken = await Api.Bots.Token.GetAsync(Credentials);
                var graphToken = await Api.Bots.Token.GetGraphAsync(Credentials);

                BotToken = new JsonWebToken(botToken.AccessToken);
                GraphToken = new JsonWebToken(graphToken.AccessToken);
            }

            foreach (var plugin in Plugins)
            {
                await plugin.OnInit(this);
            }

            foreach (var plugin in Plugins)
            {
                await plugin.OnStart(this, new() { Logger = Logger });
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