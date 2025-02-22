using System.Text.Json.Serialization;

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

    public async Task<GetResponseBody> GetAsync(Auth.ClientCredentials credentials)
    {
        var tenantId = credentials.TenantId ?? "botframework.com";
        var req = credentials.OnRequest(HttpRequest.Post(
            $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"
        ));

        var res = await _http.SendAsync<GetResponseBody>(req);
        return res.Body;
    }

    public class GetResponseBody
    {
        [JsonPropertyName("token_type")]
        [JsonPropertyOrder(0)]
        public required string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        [JsonPropertyOrder(1)]
        public required int ExpiresIn { get; set; }

        [JsonPropertyName("access_token")]
        [JsonPropertyOrder(2)]
        public required string AccessToken { get; set; }
    }
}