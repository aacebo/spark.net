using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class UserClient : Client
{
    public UserTokenClient Token { get; }

    public UserClient() : base()
    {
        Token = new UserTokenClient(_http);
    }

    public UserClient(IHttpClient client) : base(client)
    {
        Token = new UserTokenClient(_http);
    }

    public UserClient(IHttpClientOptions options) : base(options)
    {
        Token = new UserTokenClient(_http);
    }

    public UserClient(IHttpClientFactory factory) : base(factory)
    {
        Token = new UserTokenClient(_http);
    }
}