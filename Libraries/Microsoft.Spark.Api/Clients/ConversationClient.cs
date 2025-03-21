using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class ConversationClient : Client
{
    public readonly string ServiceUrl;
    public readonly ActivityClient Activities;

    public ConversationClient(string serviceUrl) : base()
    {
        ServiceUrl = serviceUrl;
        Activities = new ActivityClient(serviceUrl, _http);
    }

    public ConversationClient(string serviceUrl, IHttpClient client) : base(client)
    {
        ServiceUrl = serviceUrl;
        Activities = new ActivityClient(serviceUrl, _http);
    }

    public ConversationClient(string serviceUrl, IHttpClientOptions options) : base(options)
    {
        ServiceUrl = serviceUrl;
        Activities = new ActivityClient(serviceUrl, _http);
    }

    public ConversationClient(string serviceUrl, IHttpClientFactory factory) : base(factory)
    {
        ServiceUrl = serviceUrl;
        Activities = new ActivityClient(serviceUrl, _http);
    }
}