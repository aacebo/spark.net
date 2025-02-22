using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class ApiClient : Client
{
    public BotClient Bot { get; }

    public ApiClient() : base()
    {
        Bot = new BotClient(_http);
    }

    public ApiClient(IHttpClient client) : base(client)
    {
        Bot = new BotClient(client);
    }

    public ApiClient(IHttpClientFactory factory) : base(factory)
    {
        Bot = new BotClient(factory);
    }
}