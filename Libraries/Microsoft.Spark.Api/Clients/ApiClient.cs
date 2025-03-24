using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class ApiClient : Client
{
    public readonly string ServiceUrl;
    public readonly BotClient Bots;
    public readonly ConversationClient Conversations;
    public readonly UserClient Users;

    public ApiClient(string serviceUrl) : base()
    {
        ServiceUrl = serviceUrl;
        Bots = new BotClient(_http);
        Conversations = new ConversationClient(serviceUrl, _http);
        Users = new UserClient(_http);
    }

    public ApiClient(string serviceUrl, IHttpClient client) : base(client)
    {
        ServiceUrl = serviceUrl;
        Bots = new BotClient(_http);
        Conversations = new ConversationClient(serviceUrl, _http);
        Users = new UserClient(_http);
    }

    public ApiClient(string serviceUrl, IHttpClientOptions options) : base(options)
    {
        ServiceUrl = serviceUrl;
        Bots = new BotClient(_http);
        Conversations = new ConversationClient(serviceUrl, _http);
        Users = new UserClient(_http);
    }

    public ApiClient(string serviceUrl, IHttpClientFactory factory) : base(factory)
    {
        ServiceUrl = serviceUrl;
        Bots = new BotClient(_http);
        Conversations = new ConversationClient(serviceUrl, _http);
        Users = new UserClient(_http);
    }
}