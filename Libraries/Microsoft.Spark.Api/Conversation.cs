using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

public class Conversation
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("tenantId")]
    [JsonPropertyOrder(1)]
    public string? TenantId { get; set; }

    [JsonPropertyName("conversationType")]
    [JsonPropertyOrder(2)]
    public required ConversationType Type { get; set; }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(3)]
    public string? Name { get; set; }

    [JsonPropertyName("isGroup")]
    [JsonPropertyOrder(4)]
    public bool? IsGroup { get; set; }
}

[JsonConverter(typeof(JsonConverter<ConversationType>))]
public class ConversationType(string value) : StringEnum(value)
{
    public static readonly ConversationType Personal = new("personal");
    public bool IsPersonal => Personal.Equals(Value);

    public static readonly ConversationType GroupChat = new("groupChat");
    public bool IsGroupChat => GroupChat.Equals(Value);
}

public class ConversationReference
{
    [JsonPropertyName("activityId")]
    [JsonPropertyOrder(0)]
    public string? ActivityId { get; set; }

    [JsonPropertyName("user")]
    [JsonPropertyOrder(1)]
    public Account? User { get; set; }

    [JsonPropertyName("locale")]
    [JsonPropertyOrder(2)]
    public string? Locale { get; set; }

    [JsonPropertyName("bot")]
    [JsonPropertyOrder(3)]
    public required Account Bot { get; set; }

    [JsonPropertyName("conversation")]
    [JsonPropertyOrder(4)]
    public required Conversation Conversation { get; set; }

    [JsonPropertyName("channelId")]
    [JsonPropertyOrder(5)]
    public required ChannelId ChannelId { get; set; }

    [JsonPropertyName("serviceUrl")]
    [JsonPropertyOrder(6)]
    public required string ServiceUrl { get; set; }
}