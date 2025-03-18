using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;
using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

[JsonConverter(typeof(JsonConverter<ActivityType>))]
public partial class ActivityType(string value) : StringEnum(value)
{
}

[JsonConverter(typeof(ActivityJsonConverter))]
public class Activity(string type)
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; } = "";

    [JsonPropertyName("type")]
    [JsonPropertyOrder(10)]
    public ActivityType Type { get; init; } = new(type);

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
    public ChannelData? ChannelData { get; set; }

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

    public Activity ToActivity()
    {
        if (Type.IsTyping) return ToTyping();
        if (Type.IsInstallUpdate) return ToInstallUpdate();
        if (Type.IsMessage) return ToMessage();
        if (Type.IsMessageDelete) return ToMessageDelete();
        if (Type.IsMessageUpdate) return ToMessageUpdate();
        if (Type.IsMessageReaction) return ToMessageReaction();
        if (Type.IsConversationUpdate) return ToConversationUpdate();
        if (Type.IsEndOfConversation) return ToEndOfConversation();

        return this;
    }

    public TypingActivity ToTyping()
    {
        return (TypingActivity)this;
    }

    public InstallUpdateActivity ToInstallUpdate()
    {
        return (InstallUpdateActivity)this;
    }

    public MessageActivity ToMessage()
    {
        return (MessageActivity)this;
    }

    public MessageUpdateActivity ToMessageUpdate()
    {
        return (MessageUpdateActivity)this;
    }

    public MessageDeleteActivity ToMessageDelete()
    {
        return (MessageDeleteActivity)this;
    }

    public MessageReactionActivity ToMessageReaction()
    {
        return (MessageReactionActivity)this;
    }

    public ConversationUpdateActivity ToConversationUpdate()
    {
        return (ConversationUpdateActivity)this;
    }

    public EndOfConversationActivity ToEndOfConversation()
    {
        return (EndOfConversationActivity)this;
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

public class ActivityJsonConverter : JsonConverter<Activity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override Activity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "message" => JsonSerializer.Deserialize<MessageActivity>(element.ToString(), options),
            "messageUpdate" => JsonSerializer.Deserialize<MessageUpdateActivity>(element.ToString(), options),
            "messageDelete" => JsonSerializer.Deserialize<MessageDeleteActivity>(element.ToString(), options),
            "messageReaction" => JsonSerializer.Deserialize<MessageReactionActivity>(element.ToString(), options),
            "conversationUpdate" => JsonSerializer.Deserialize<ConversationUpdateActivity>(element.ToString(), options),
            "endOfConversation" => JsonSerializer.Deserialize<EndOfConversationActivity>(element.ToString(), options),
            "installationUpdate" => JsonSerializer.Deserialize<InstallUpdateActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<Activity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, Activity value, JsonSerializerOptions options)
    {
        if (value is TypingActivity typing)
        {
            JsonSerializer.Serialize(writer, typing, options);
            return;
        }

        if (value is MessageActivity message)
        {
            JsonSerializer.Serialize(writer, message, options);
            return;
        }

        if (value is MessageUpdateActivity messageUpdate)
        {
            JsonSerializer.Serialize(writer, messageUpdate, options);
            return;
        }

        if (value is MessageDeleteActivity messageDelete)
        {
            JsonSerializer.Serialize(writer, messageDelete, options);
            return;
        }

        if (value is MessageReactionActivity messageReaction)
        {
            JsonSerializer.Serialize(writer, messageReaction, options);
            return;
        }

        if (value is ConversationUpdateActivity conversationUpdate)
        {
            JsonSerializer.Serialize(writer, conversationUpdate, options);
            return;
        }

        if (value is EndOfConversationActivity endOfConversation)
        {
            JsonSerializer.Serialize(writer, endOfConversation, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}