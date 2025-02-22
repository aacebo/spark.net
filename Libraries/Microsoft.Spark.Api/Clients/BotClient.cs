using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class BotClient : Client
{
    public BotTokenClient Token { get; }

    public BotClient() : base()
    {
        Token = new BotTokenClient(_http);
    }

    public BotClient(IHttpClient client) : base(client)
    {
        Token = new BotTokenClient(client);
    }

    public BotClient(IHttpClientFactory factory) : base(factory)
    {
        Token = new BotTokenClient(factory);
    }
}