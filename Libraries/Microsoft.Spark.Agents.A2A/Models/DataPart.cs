using System.Text.Json.Serialization;

namespace Microsoft.Spark.Agents.A2A.Models;

public class DataPart : IPart
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(0)]
    public string Type => "data";

    [JsonPropertyName("data")]
    [JsonPropertyOrder(1)]
    public required object Data { get; set; }

    [JsonPropertyName("metadata")]
    [JsonPropertyOrder(2)]
    public IDictionary<string, object>? MetaData { get; set; }
}