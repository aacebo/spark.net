using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class Tenant
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }
}