using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

/// <summary>
/// Defines the structure that arrives in the Activity.Value for Invoke activity with
/// Name of 'adaptiveCard/action'.
/// </summary>
public class AdaptiveCardInvokeValue
{
    /// <summary>
    /// the action of this adaptive card invoke action value.
    /// </summary>
    [JsonPropertyName("action")]
    [JsonPropertyOrder(0)]
    public required AdaptiveCardInvokeAction Action { get; set; }

    /// <summary>
    /// the token exchange request for this adaptive card invoke action value.
    /// </summary>
    [JsonPropertyName("authentication")]
    [JsonPropertyOrder(1)]
    public TokenExchangeInvokeRequest? Authentication { get; set; }

    /// <summary>
    /// for this adaptive card invoke action value.
    /// </summary>
    [JsonPropertyName("state")]
    [JsonPropertyOrder(2)]
    public string? State { get; set; }

    /// <summary>
    /// What triggered the action
    /// </summary>
    [JsonPropertyName("trigger")]
    [JsonPropertyOrder(3)]
    public string? Trigger { get; set; }
}