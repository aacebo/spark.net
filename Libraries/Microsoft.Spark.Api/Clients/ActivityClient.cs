using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class ActivityClient : Client
{
    public readonly string ServiceUrl;

    public ActivityClient(string serviceUrl) : base()
    {
        ServiceUrl = serviceUrl;
    }

    public ActivityClient(string serviceUrl, IHttpClient client) : base(client)
    {
        ServiceUrl = serviceUrl;
    }

    public ActivityClient(string serviceUrl, IHttpRequestOptions options) : base(options)
    {
        ServiceUrl = serviceUrl;
    }

    public ActivityClient(string serviceUrl, IHttpClientFactory factory) : base(factory)
    {
        ServiceUrl = serviceUrl;
    }

    public async Task<Resource> CreateAsync(string conversationId, Activity activity)
    {
        var req = HttpRequest.Post(
            $"{ServiceUrl}/v3/conversations/{conversationId}/activities",
            body: activity
        );

        var res = await _http.SendAsync<Resource>(req);
        return res.Body;
    }

    public async Task<Resource> UpdateAsync(string conversationId, string id, Activity activity)
    {
        var req = HttpRequest.Put(
            $"{ServiceUrl}/v3/conversations/{conversationId}/activities/{id}",
            body: activity
        );

        var res = await _http.SendAsync<Resource>(req);
        return res.Body;
    }

    public async Task<Resource> ReplyAsync(string conversationId, string id, Activity activity)
    {
        activity.ReplyToId = id;
        var req = HttpRequest.Post(
            $"{ServiceUrl}/v3/conversations/{conversationId}/activities/{id}",
            body: activity
        );

        var res = await _http.SendAsync<Resource>(req);
        return res.Body;
    }

    public async Task DeleteAsync(string conversationId, string id)
    {
        var req = HttpRequest.Delete(
            $"{ServiceUrl}/v3/conversations/{conversationId}/activities/{id}"
        );

        await _http.SendAsync(req);
    }
}