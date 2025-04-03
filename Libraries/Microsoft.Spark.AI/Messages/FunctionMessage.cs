using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI.Messages;

public class FunctionMessage : IMessage<string?>
{
    [JsonPropertyName("role")]
    [JsonPropertyOrder(0)]
    public Role Role => Role.Function;

    [JsonPropertyName("content")]
    [JsonPropertyOrder(1)]
    public string? Content { get; set; }

    [JsonPropertyName("function_id")]
    [JsonPropertyOrder(2)]
    public required string FunctionId { get; set; }
}