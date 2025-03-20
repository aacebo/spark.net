using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.TokenExchange;

/// <summary>
/// An interface representing TokenExchangeRequest.
/// </summary>
public class Request
{
    /// <summary>
    /// the request uri
    /// </summary>
    [JsonPropertyName("uri")]
    [JsonPropertyOrder(0)]
    public string? Uri { get; set; }

    /// <summary>
    /// the request token
    /// </summary>
    [JsonPropertyName("token")]
    [JsonPropertyOrder(1)]
    public string? Token { get; set; }
}