using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public interface IChannelData
{
    [JsonPropertyName("channel")]
    [JsonPropertyOrder(0)]
    public Channel? Channel { get; set; }

    [JsonPropertyName("eventType")]
    [JsonPropertyOrder(1)]
    public string? EventType { get; set; }

    [JsonPropertyName("team")]
    [JsonPropertyOrder(2)]
    public Team? Team { get; set; }

    [JsonPropertyName("tenant")]
    [JsonPropertyOrder(3)]
    public Tenant? Tenant { get; set; }

    [JsonPropertyName("notification")]
    [JsonPropertyOrder(4)]
    public Notification? Notification { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?> Properties { get; set; }
}

public class ChannelData : IChannelData
{
    [JsonPropertyName("channel")]
    [JsonPropertyOrder(0)]
    public Channel? Channel { get; set; }

    [JsonPropertyName("eventType")]
    [JsonPropertyOrder(1)]
    public string? EventType { get; set; }

    [JsonPropertyName("team")]
    [JsonPropertyOrder(2)]
    public Team? Team { get; set; }

    [JsonPropertyName("tenant")]
    [JsonPropertyOrder(3)]
    public Tenant? Tenant { get; set; }

    [JsonPropertyName("notification")]
    [JsonPropertyOrder(4)]
    public Notification? Notification { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?> Properties { get; set; } = [];
}