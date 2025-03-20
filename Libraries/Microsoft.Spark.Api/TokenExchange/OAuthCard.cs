using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.TokenExchange;

/// <summary>
/// A card representing a request to perform a sign in via OAuth
/// </summary>
public class OAuthCard
{
    /// <summary>
    /// Text for signin request
    /// </summary>
    [JsonPropertyName("text")]
    [JsonPropertyOrder(0)]
    public required string Text { get; set; }

    /// <summary>
    /// The name of the registered connection
    /// </summary>
    [JsonPropertyName("connectionName")]
    [JsonPropertyOrder(1)]
    public required string ConnectionName { get; set; }

    /// <summary>
    /// The token exchange resource for single sign on
    /// </summary>
    [JsonPropertyName("tokenExchangeResource")]
    [JsonPropertyOrder(2)]
    public Resource? TokenExchangeResource { get; set; }

    /// <summary>
    /// The token for directly post a token to token service
    /// </summary>
    [JsonPropertyName("tokenPostResource")]
    [JsonPropertyOrder(3)]
    public Token.PostResource? TokenPostResource { get; set; }

    /// <summary>
    /// Action to use to perform signin
    /// </summary>
    public IList<Cards.Action> Buttons { get; set; } = [];
}