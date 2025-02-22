using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class Team
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("aadGroupId")]
    [JsonPropertyOrder(1)]
    public string? AadGroupId { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(2)]
    public TeamType? Type { get; set; }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(3)]
    public string? Name { get; set; }

    [JsonPropertyName("channelCount")]
    [JsonPropertyOrder(4)]
    public int? ChannelCount { get; set; }

    [JsonPropertyName("memberCount")]
    [JsonPropertyOrder(5)]
    public int? MemberCount { get; set; }
}

[JsonConverter(typeof(JsonConverter<TeamType>))]
public class TeamType(string value) : Common.StringEnum(value)
{
    public static readonly TeamType Standard = new("standard");
    public bool IsStandard => Standard.Equals(Value);

    public static readonly TeamType SharedChannel = new("sharedChannel");
    public bool IsSharedChannel => SharedChannel.Equals(Value);

    public static readonly TeamType PrivateChannel = new("privateChannel");
    public bool IsPrivateChannel => PrivateChannel.Equals(Value);
}