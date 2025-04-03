using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI.Messages;

public class DeveloperMessage : IMessage<string>
{
    [JsonPropertyName("role")]
    [JsonPropertyOrder(0)]
    public Role Role => Role.Developer;

    [JsonPropertyName("content")]
    [JsonPropertyOrder(1)]
    public string Content { get; set; }

    [JsonConstructor]
    public DeveloperMessage(string content)
    {
        Content = content;
    }
}