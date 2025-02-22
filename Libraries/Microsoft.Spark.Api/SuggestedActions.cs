using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class SuggestedActions
{
    [JsonPropertyName("to")]
    [JsonPropertyOrder(0)]
    public IList<string> To { get; set; } = [];
}