using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

/// <summary>
/// An interface representing AppBasedLinkQuery.
/// </summary>
public class AppBasedQueryLink
{
    /// <summary>
    /// Url queried by user
    /// </summary>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(0)]
    public string? Url { get; set; }

    /// <summary>
    /// State is the magic code for OAuth Flow
    /// </summary>
    [JsonPropertyName("state")]
    [JsonPropertyOrder(1)]
    public string? State { get; set; }
}