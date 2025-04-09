using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class BotTokenClient : Client
{
    public BotTokenClient(CancellationToken cancellationToken = default) : base(cancellationToken)
    {

    }

    public BotTokenClient(IHttpClient client, CancellationToken cancellationToken = default) : base(client, cancellationToken)
    {

    }

    public BotTokenClient(IHttpClientOptions options, CancellationToken cancellationToken = default) : base(options, cancellationToken)
    {

    }

    public BotTokenClient(IHttpClientFactory factory, CancellationToken cancellationToken = default) : base(factory, cancellationToken)
    {

    }

    public async Task<ITokenResponse> GetAsync(IHttpCredentials credentials)
    {
        return await credentials.Resolve(_http, ["https://api.botframework.com/.default"], _cancellationToken);
    }

    public async Task<ITokenResponse> GetGraphAsync(IHttpCredentials credentials)
    {
        return await credentials.Resolve(_http, ["https://graph.microsoft.com/.default"], _cancellationToken);
    }
}