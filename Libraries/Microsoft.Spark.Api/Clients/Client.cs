using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public abstract class Client
{
    protected IHttpClient _http;

    public Client()
    {
        _http = new Common.Http.HttpClient();
    }

    public Client(IHttpClient client)
    {
        _http = client;
    }

    public Client(IHttpRequestOptions options)
    {
        _http = new Common.Http.HttpClient(options);
    }

    public Client(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("default");
    }
}