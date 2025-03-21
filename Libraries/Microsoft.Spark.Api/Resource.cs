using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

/// <summary>
/// A response containing a resource ID
/// </summary>
public class Resource
{
    /// <summary>
    /// Id of the resource
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }
}