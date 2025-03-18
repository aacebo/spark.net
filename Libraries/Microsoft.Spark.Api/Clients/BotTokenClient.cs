using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class BotTokenClient : Client
{
    public BotTokenClient() : base()
    {

    }

    public BotTokenClient(IHttpClient client) : base(client)
    {

    }

    public BotTokenClient(IHttpClientFactory factory) : base(factory)
    {

    }

    public async Task<ITokenResponse> GetAsync(IHttpCredentials credentials)
    {
        return await credentials.Resolve(_http);
    }
}