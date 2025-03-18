using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Activities.Conversation;
using Microsoft.Spark.Api.Activities.Message;
using Microsoft.Spark.Api.Entities;

namespace Microsoft.Spark.Api.Activities;

[JsonConverter(typeof(ActivityJsonConverter))]
public interface IActivity
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(10)]
    public string Type { get; init; }

    [JsonPropertyName("replyToId")]
    [JsonPropertyOrder(20)]
    public string? ReplyToId { get; set; }

    [JsonPropertyName("channelId")]
    [JsonPropertyOrder(30)]
    public ChannelId ChannelId { get; set; }

    [JsonPropertyName("from")]
    [JsonPropertyOrder(40)]
    public Account From { get; set; }

    [JsonPropertyName("recipient")]
    [JsonPropertyOrder(50)]
    public Account Recipient { get; set; }

    [JsonPropertyName("conversation")]
    [JsonPropertyOrder(60)]
    public Api.Conversation Conversation { get; set; }

    [JsonPropertyName("relatesTo")]
    [JsonPropertyOrder(70)]
    public ConversationReference? RelatesTo { get; set; }

    [JsonPropertyName("serviceUrl")]
    [JsonPropertyOrder(80)]
    public string? ServiceUrl { get; set; }

    [JsonPropertyName("locale")]
    [JsonPropertyOrder(90)]
    public string? Locale { get; set; }

    [JsonPropertyName("timestamp")]
    [JsonPropertyOrder(100)]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("localTimestamp")]
    [JsonPropertyOrder(110)]
    public DateTime? LocalTimestamp { get; set; }

    [JsonPropertyName("entities")]
    [JsonPropertyOrder(120)]
    public IList<IEntity>? Entities { get; set; }

    [JsonPropertyName("channelData")]
    [JsonPropertyOrder(130)]
    public IChannelData? ChannelData { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?> Properties { get; set; }
}

public class Activity : IActivity
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; } = "";

    [JsonPropertyName("type")]
    [JsonPropertyOrder(10)]
    public string Type { get; init; } = string.Empty;

    [JsonPropertyName("replyToId")]
    [JsonPropertyOrder(20)]
    public string? ReplyToId { get; set; }

    [JsonPropertyName("channelId")]
    [JsonPropertyOrder(30)]
    public ChannelId ChannelId { get; set; } = ChannelId.MsTeams;

    [JsonPropertyName("from")]
    [JsonPropertyOrder(40)]
    public required Account From { get; set; }

    [JsonPropertyName("recipient")]
    [JsonPropertyOrder(50)]
    public required Account Recipient { get; set; }

    [JsonPropertyName("conversation")]
    [JsonPropertyOrder(60)]
    public required Api.Conversation Conversation { get; set; }

    [JsonPropertyName("relatesTo")]
    [JsonPropertyOrder(70)]
    public ConversationReference? RelatesTo { get; set; }

    [JsonPropertyName("serviceUrl")]
    [JsonPropertyOrder(80)]
    public string? ServiceUrl { get; set; }

    [JsonPropertyName("locale")]
    [JsonPropertyOrder(90)]
    public string? Locale { get; set; }

    [JsonPropertyName("timestamp")]
    [JsonPropertyOrder(100)]
    public DateTime? Timestamp { get; set; }

    [JsonPropertyName("localTimestamp")]
    [JsonPropertyOrder(110)]
    public DateTime? LocalTimestamp { get; set; }

    [JsonPropertyName("entities")]
    [JsonPropertyOrder(120)]
    public IList<IEntity>? Entities { get; set; }

    [JsonPropertyName("channelData")]
    [JsonPropertyOrder(130)]
    public IChannelData? ChannelData { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?> Properties { get; set; } = [];

    public Activity AddEntity(IEntity entity)
    {
        if (Entities == null)
        {
            Entities = [];
        }

        Entities.Add(entity);
        return this;
    }

    public Activity AddAIGenerated()
    {
        return AddEntity(new MessageEntity()
        {
            Type = "https://schema.org/Message",
            OType = "Message",
            OContext = "https://schema.org",
            AdditionalType = ["AIGeneratedContent"]
        });
    }

    public Activity AddCitation(int position, CitationAppearance appearance)
    {
        return AddEntity(new CitationEntity()
        {
            Position = position,
            Appearance = appearance.ToDocument()
        });
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}

public class ActivityJsonConverter : JsonConverter<IActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override IActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (!element.TryGetProperty("type", out JsonElement property))
        {
            throw new JsonException("activity must have a 'type' property");
        }

        var type = property.Deserialize<string>(options);

        if (type == null)
        {
            throw new JsonException("failed to deserialize activity 'type' property");
        }

        return type switch
        {
            "typing" => JsonSerializer.Deserialize<TypingActivity>(element.ToString(), options),
            "message" or "messageUpdate" or "messageDelete" or "messageReaction" => JsonSerializer.Deserialize<IMessageActivityBase>(element.ToString(), options),
            "conversationUpdate" or "endOfConversation" => JsonSerializer.Deserialize<IConversationActivityBase>(element.ToString(), options),
            "installationUpdate" => JsonSerializer.Deserialize<IInstallUpdateActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<Activity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, IActivity value, JsonSerializerOptions options)
    {
        if (value is ITypingActivity typing)
        {
            JsonSerializer.Serialize(writer, typing, options);
            return;
        }

        if (value is IMessageActivityBase message)
        {
            JsonSerializer.Serialize(writer, message, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}