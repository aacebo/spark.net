using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Activities;
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

    public async Task<ConversationResource> CreateAsync(CreateRequest request)
    {
        var req = HttpRequest.Post($"{ServiceUrl}v3/conversations", body: request);
        var res = await _http.SendAsync<ConversationResource>(req);
        return res.Body;
    }

    public class CreateRequest
    {
        [JsonPropertyName("isGroup")]
        [JsonPropertyOrder(0)]
        public bool? IsGroup { get; set; }

        [JsonPropertyName("bot")]
        [JsonPropertyOrder(1)]
        public Account? Bot { get; set; }

        [JsonPropertyName("members")]
        [JsonPropertyOrder(2)]
        public IList<Account>? Members { get; set; }

        [JsonPropertyName("topicName")]
        [JsonPropertyOrder(3)]
        public string? TopicName { get; set; }

        [JsonPropertyName("tenantId")]
        [JsonPropertyOrder(4)]
        public string? TenantId { get; set; }

        [JsonPropertyName("activity")]
        [JsonPropertyOrder(5)]
        public IActivity? Activity { get; set; }

        [JsonPropertyName("channelData")]
        [JsonPropertyOrder(6)]
        public IDictionary<string, object>? ChannelData { get; set; }
    }
}