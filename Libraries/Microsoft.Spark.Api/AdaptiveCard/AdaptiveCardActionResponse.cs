using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

/// <summary>
/// Defines the structure that is returned as the result of an Invoke activity with
/// Name of 'adaptiveCard/action'.
/// </summary>
public class AdaptiveCardActionResponse(ContentType contentType)
{
    /// <summary>
    /// The Card Action response status code.
    /// </summary>
    [JsonPropertyName("statusCode")]
    [JsonPropertyOrder(0)]
    public required int StatusCode { get; set; }

    /// <summary>
    /// The type of this response.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonPropertyOrder(1)]
    public ContentType Type { get; set; } = contentType;

    /// <summary>
    /// the response value
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(2)]
    public object? Value { get; set; }
}