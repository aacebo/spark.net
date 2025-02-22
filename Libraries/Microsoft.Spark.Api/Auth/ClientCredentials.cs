using System.Text.Json.Serialization;

using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Auth;

public class ClientCredentials : IHttpCredentials
{
    [JsonPropertyName("clientId")]
    [JsonPropertyOrder(0)]
    public string ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    [JsonPropertyOrder(1)]
    public string ClientSecret { get; set; }

    [JsonPropertyName("tenantId")]
    [JsonPropertyOrder(2)]
    public string? TenantId { get; set; }

    public ClientCredentials(string clientId, string clientSecret)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    public ClientCredentials(string clientId, string clientSecret, string tenantId)
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        TenantId = tenantId;
    }

    public IHttpRequest OnRequest(IHttpRequest request)
    {
        request.Headers.Add("Content-Type", ["application/x-www-form-urlencoded"]);
        request.Body = QueryString.Serialize(new
        {
            grant_type = "client_credentials",
            client_id = ClientId,
            client_secret = ClientSecret,
            scope = "https://api.botframework.com/.default"
        });

        return request;
    }
}