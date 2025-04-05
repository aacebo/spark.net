using System.Reflection;

using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Api.Clients;
using Microsoft.Spark.Apps.Plugins;
using Microsoft.Spark.Common.Http;
using Microsoft.Spark.Common.Logging;
using Microsoft.Spark.Common.Storage;

namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public ILogger Logger { get; }
    public IStorage<string, object> Storage { get; }
    public ApiClient Api { get; }
    public IHttpClient Client { get; }
    public IHttpCredentials? Credentials { get; }
    public IToken? BotToken { get; }
    public IToken? GraphToken { get; }

    /// <summary>
    /// start the app
    /// </summary>
    public Task Start();

    /// <summary>
    /// send an activity to the conversation
    /// </summary>
    /// <param name="activity">activity activity to send</param>
    public Task<T> Send<T>(string conversationId, T activity, string? serviceUrl = null) where T : IActivity;

    /// <summary>
    /// send a message activity to the conversation
    /// </summary>
    /// <param name="text">the text to send</param>
    public Task<MessageActivity> Send(string conversationId, string text, string? serviceUrl = null);

    /// <summary>
    /// send a message activity with a card attachment
    /// </summary>
    /// <param name="card">the card to send as an attachment</param>
    public Task<MessageActivity> Send(string conversationId, Cards.Card card, string? serviceUrl = null);
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
        ErrorEvent = (_, sender, exception, context) => OnErrorEvent(sender, exception, context);
        StartEvent = (_, _) => OnStartEvent();
        ActivityEvent = (_, _) => Task.Run(() => { });
        ActivitySentEvent = (_, plugin, args) => OnActivitySentEvent(plugin, args);
        ActivityResponseEvent = (_, plugin, args) => OnActivityResponseEvent(plugin, args);

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
        OnTokenExchange(OnTokenExchangeActivity);
        OnVerifyState(OnVerifyStateActivity);
    }

    /// <summary>
    /// start the app
    /// </summary>
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

            Logger.Debug(Id);
            Logger.Debug(Name);

            foreach (var plugin in Plugins)
            {
                await plugin.OnInit(this);
            }

            foreach (var plugin in Plugins)
            {
                await plugin.OnStart(this);
            }

            await StartEvent(this, Logger);
        }
        catch (Exception err)
        {
            await ErrorEvent(this, null, err, null);
        }
    }

    /// <summary>
    /// send an activity to the conversation
    /// </summary>
    /// <param name="activity">activity activity to send</param>
    public async Task<T> Send<T>(string conversationId, T activity, string? serviceUrl = null) where T : IActivity
    {
        if (Id == null || Name == null)
        {
            throw new InvalidOperationException("app not started");
        }

        var reference = new ConversationReference()
        {
            ChannelId = ChannelId.MsTeams,
            ServiceUrl = serviceUrl ?? Api.ServiceUrl,
            Bot = new()
            {
                Id = Id,
                Name = Name,
                Role = Role.Bot
            },
            Conversation = new()
            {
                Id = conversationId,
                Type = ConversationType.Personal
            }
        };

        var sender = Plugins.Where(plugin => plugin is ISender).Select(plugin => plugin as ISender).First();

        if (sender == null)
        {
            throw new Exception("no plugin that can send activities was found");
        }

        var res = await sender.Send(activity, reference);

        await OnActivitySentEvent(sender, res, new()
        {
            Bot = reference.Bot,
            ChannelId = reference.ChannelId,
            Conversation = reference.Conversation,
            ServiceUrl = reference.ServiceUrl,
            ActivityId = reference.ActivityId,
            Locale = reference.Locale,
            User = reference.User
        }).ConfigureAwait(false);

        return res;
    }

    /// <summary>
    /// send a message activity to the conversation
    /// </summary>
    /// <param name="text">the text to send</param>
    public async Task<MessageActivity> Send(string conversationId, string text, string? serviceUrl = null)
    {
        return await Send(conversationId, new MessageActivity(text), serviceUrl);
    }

    /// <summary>
    /// send a message activity with a card attachment
    /// </summary>
    /// <param name="card">the card to send as an attachment</param>
    public async Task<MessageActivity> Send(string conversationId, Cards.Card card, string? serviceUrl = null)
    {
        return await Send(conversationId, new MessageActivity().AddAttachment(card), serviceUrl);
    }
}