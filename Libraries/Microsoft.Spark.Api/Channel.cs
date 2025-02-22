using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class Channel
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(1)]
    public ChannelType? Type { get; set; }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(2)]
    public string? Name { get; set; }
}

[JsonConverter(typeof(JsonConverter<ChannelType>))]
public class ChannelType(string value) : Common.StringEnum(value)
{
    public static readonly ChannelType Standard = new("standard");
    public bool IsStandard => Standard.Equals(Value);

    public static readonly ChannelType Shared = new("shared");
    public bool IsShared => Shared.Equals(Value);

    public static readonly ChannelType Private = new("private");
    public bool IsPrivate => Private.Equals(Value);
}