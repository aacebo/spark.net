using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class BotClient : Client
{
    public BotTokenClient Token { get; }
    public BotSignInClient SignIn { get; }

    public BotClient() : base()
    {
        Token = new BotTokenClient(_http);
        SignIn = new BotSignInClient(_http);
    }

    public BotClient(IHttpClient client) : base(client)
    {
        Token = new BotTokenClient(_http);
        SignIn = new BotSignInClient(_http);
    }

    public BotClient(IHttpClientOptions options) : base(options)
    {
        Token = new BotTokenClient(_http);
        SignIn = new BotSignInClient(_http);
    }

    public BotClient(IHttpClientFactory factory) : base(factory)
    {
        Token = new BotTokenClient(_http);
        SignIn = new BotSignInClient(_http);
    }
}