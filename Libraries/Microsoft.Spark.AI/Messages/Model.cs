using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI.Messages;

public class ModelMessage : IMessage
{
    [JsonPropertyName("role")]
    [JsonPropertyOrder(0)]
    public Role Role => Role.Model;

    [JsonPropertyName("content")]
    [JsonPropertyOrder(1)]
    public string? Content { get; set; }

    [JsonPropertyName("function_calls")]
    [JsonPropertyOrder(2)]
    public IList<FunctionCall>? FunctionCalls { get; set; }
}

/// <summary>
/// represents a models request to
/// invoke a function
/// </summary>
public class FunctionCall
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    public required string Name { get; set; }

    [JsonPropertyName("arguments")]
    [JsonPropertyOrder(2)]
    public object? Arguments { get; set; }
}