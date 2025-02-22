using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Entities;

public interface IMentionEntity : IEntity
{
    [JsonPropertyName("mentioned")]
    [JsonPropertyOrder(3)]
    public Account Mentioned { get; set; }

    [JsonPropertyName("text")]
    [JsonPropertyOrder(4)]
    public string? Text { get; set; }
}

public class MentionEntity : Entity, IMentionEntity
{
    [JsonPropertyName("mentioned")]
    [JsonPropertyOrder(3)]
    public required Account Mentioned { get; set; }

    [JsonPropertyName("text")]
    [JsonPropertyOrder(4)]
    public string? Text { get; set; }

    public MentionEntity() : base("mention") { }
}