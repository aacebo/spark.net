using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

/// <summary>
/// An interface representing TokenPostResource.
/// </summary>
public class TokenPostResource
{
    [JsonPropertyName("sasUrl")]
    [JsonPropertyOrder(0)]
    public string? SasUrl { get; set; }
}