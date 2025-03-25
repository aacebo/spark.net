using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Clients;

public class UserTokenClient : Client
{
    public UserTokenClient() : base()
    {

    }

    public UserTokenClient(IHttpClient client) : base(client)
    {

    }

    public UserTokenClient(IHttpClientOptions options) : base(options)
    {

    }

    public UserTokenClient(IHttpClientFactory factory) : base(factory)
    {

    }

    public async Task<Token.Response> GetAsync(GetTokenRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Get($"https://token.botframework.com/api/usertoken/GetToken?{query}");
        var res = await _http.SendAsync<Token.Response>(req);
        return res.Body;
    }

    public async Task<IDictionary<string, Token.Response>> GetAadAsync(GetAadTokenRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Post($"https://token.botframework.com/api/usertoken/GetAadTokens?{query}", body: request);
        var res = await _http.SendAsync<IDictionary<string, TokenResponse>>(req);
        return (IDictionary<string, Token.Response>)res.Body;
    }

    public async Task<IList<Token.Status>> GetStatusAsync(GetTokenStatusRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Get($"https://token.botframework.com/api/usertoken/GetTokenStatus?{query}");
        var res = await _http.SendAsync<IList<Token.Status>>(req);
        return res.Body;
    }

    public async Task SignOutAsync(SignOutRequest request)
    {
        var query = QueryString.Serialize(request);
        var req = HttpRequest.Delete($"https://token.botframework.com/api/usertoken/SignOut?{query}");
        await _http.SendAsync(req);
    }

    public async Task<Token.Response> ExchangeAsync(ExchangeTokenRequest request)
    {
        var query = QueryString.Serialize(new
        {
            userId = request.UserId,
            connectionName = request.ConnectionName,
            channelId = request.ChannelId
        });

        var req = HttpRequest.Post($"https://token.botframework.com/api/usertoken/exchange?{query}", request.GetBody());
        var res = await _http.SendAsync<Token.Response>(req);
        return res.Body;
    }

    public class GetTokenRequest
    {
        [JsonPropertyName("userId")]
        [JsonPropertyOrder(0)]
        public required string UserId { get; set; }

        [JsonPropertyName("connectionName")]
        [JsonPropertyOrder(1)]
        public required string ConnectionName { get; set; }

        [JsonPropertyName("channelId")]
        [JsonPropertyOrder(2)]
        public ChannelId? ChannelId { get; set; }

        [JsonPropertyName("code")]
        [JsonPropertyOrder(3)]
        public string? Code { get; set; }
    }

    public class GetAadTokenRequest
    {
        [JsonPropertyName("userId")]
        [JsonPropertyOrder(0)]
        public required string UserId { get; set; }

        [JsonPropertyName("connectionName")]
        [JsonPropertyOrder(1)]
        public required string ConnectionName { get; set; }

        [JsonPropertyName("channelId")]
        [JsonPropertyOrder(2)]
        public required ChannelId ChannelId { get; set; }

        [JsonPropertyName("resourceUrls")]
        [JsonPropertyOrder(3)]
        public IList<string> ResourceUrls { get; set; } = [];
    }

    public class GetTokenStatusRequest
    {
        [JsonPropertyName("userId")]
        [JsonPropertyOrder(0)]
        public required string UserId { get; set; }

        [JsonPropertyName("channelId")]
        [JsonPropertyOrder(1)]
        public required ChannelId ChannelId { get; set; }

        [JsonPropertyName("includeFilter")]
        [JsonPropertyOrder(2)]
        public string? IncludeFilter { get; set; }
    }

    public class SignOutRequest
    {
        [JsonPropertyName("userId")]
        [JsonPropertyOrder(0)]
        public required string UserId { get; set; }

        [JsonPropertyName("connectionName")]
        [JsonPropertyOrder(1)]
        public required string ConnectionName { get; set; }

        [JsonPropertyName("channelId")]
        [JsonPropertyOrder(2)]
        public required ChannelId ChannelId { get; set; }
    }

    public class ExchangeTokenRequest
    {
        [JsonPropertyName("userId")]
        [JsonPropertyOrder(0)]
        public required string UserId { get; set; }

        [JsonPropertyName("connectionName")]
        [JsonPropertyOrder(1)]
        public required string ConnectionName { get; set; }

        [JsonPropertyName("channelId")]
        [JsonPropertyOrder(2)]
        public required ChannelId ChannelId { get; set; }

        [JsonPropertyName("exchangeRequest")]
        [JsonPropertyOrder(3)]
        public required TokenExchange.Request ExchangeRequest { get; set; }

        internal Body GetBody() => new() { ExchangeRequest = ExchangeRequest };

        internal class Body
        {
            [JsonPropertyName("exchangeRequest")]
            [JsonPropertyOrder(0)]
            public required TokenExchange.Request ExchangeRequest { get; set; }
        }
    }
}