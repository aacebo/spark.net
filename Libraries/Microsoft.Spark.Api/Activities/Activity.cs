using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;
using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

[JsonConverter(typeof(JsonConverter<ActivityType>))]
public partial class ActivityType(string value) : StringEnum(value)
{
    public Type ToType()
    {
        if (IsTyping) return typeof(TypingActivity);
        if (IsCommand) return typeof(CommandActivity);
        if (IsCommandResult) return typeof(CommandResultActivity);
        if (IsConversationUpdate) return typeof(ConversationUpdateActivity);
        if (IsEndOfConversation) return typeof(EndOfConversationActivity);
        if (IsInstallUpdate) return typeof(InstallUpdateActivity);
        if (IsMessage) return typeof(MessageActivity);
        if (IsMessageUpdate) return typeof(MessageUpdateActivity);
        if (IsMessageDelete) return typeof(MessageDeleteActivity);
        if (IsMessageReaction) return typeof(MessageReactionActivity);
        if (IsEvent) return typeof(EventActivity);
        if (IsInvoke) return typeof(InvokeActivity);
        return typeof(Activity);
    }

    public string ToPrettyString()
    {
        var value = ToString();
        return $"{value.First().ToString().ToUpper()}{value.AsSpan(1).ToString()}";
    }
}

public interface IActivity : IConvertible
{
    public string Id { get; set; }
    public ActivityType Type { get; init; }
    public string? ReplyToId { get; set; }
    public ChannelId ChannelId { get; set; }
    public Account From { get; set; }
    public Account Recipient { get; set; }
    public Conversation Conversation { get; set; }
    public ConversationReference? RelatesTo { get; set; }
    public string? ServiceUrl { get; set; }
    public string? Locale { get; set; }
    public DateTime? Timestamp { get; set; }
    public DateTime? LocalTimestamp { get; set; }
    public IList<IEntity>? Entities { get; set; }
    public ChannelData? ChannelData { get; set; }
    public Dictionary<string, object?> Properties { get; set; }

    public string GetPath();
    public IActivity AddEntity(IEntity entity);
    public IActivity AddAIGenerated();
    public IActivity AddCitation(int position, CitationAppearance appearance);
}

[JsonConverter(typeof(JsonConverter))]
public partial class Activity(ActivityType type) : IActivity
{
    [AllowNull]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(10)]
    public ActivityType Type { get; init; } = type;

    [JsonPropertyName("replyToId")]
    [JsonPropertyOrder(20)]
    public string? ReplyToId { get; set; }

    [JsonPropertyName("channelId")]
    [JsonPropertyOrder(30)]
    public ChannelId ChannelId { get; set; } = ChannelId.MsTeams;

    [AllowNull]
    [JsonPropertyName("from")]
    [JsonPropertyOrder(40)]
    public Account From { get; set; }

    [AllowNull]
    [JsonPropertyName("recipient")]
    [JsonPropertyOrder(50)]
    public Account Recipient { get; set; }

    [AllowNull]
    [JsonPropertyName("conversation")]
    [JsonPropertyOrder(60)]
    public Conversation Conversation { get; set; }

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

    public virtual string GetPath()
    {
        return string.Join('.', ["Activity", Type.ToPrettyString()]);
    }

    public IActivity AddEntity(IEntity entity)
    {
        if (Entities == null)
        {
            Entities = [];
        }

        Entities.Add(entity);
        return this;
    }

    public IActivity AddAIGenerated()
    {
        return AddEntity(new MessageEntity()
        {
            Type = "https://schema.org/Message",
            OType = "Message",
            OContext = "https://schema.org",
            AdditionalType = ["AIGeneratedContent"]
        });
    }

    public IActivity AddCitation(int position, CitationAppearance appearance)
    {
        return AddEntity(new CitationEntity()
        {
            Position = position,
            Appearance = appearance.ToDocument()
        });
    }

    public CommandActivity ToCommand() => (CommandActivity)this;
    public CommandResultActivity ToCommandResult() => (CommandResultActivity)this;
    public TypingActivity ToTyping() => (TypingActivity)this;
    public InstallUpdateActivity ToInstallUpdate() => (InstallUpdateActivity)this;
    public MessageActivity ToMessage() => (MessageActivity)this;
    public MessageUpdateActivity ToMessageUpdate() => (MessageUpdateActivity)this;
    public MessageDeleteActivity ToMessageDelete() => (MessageDeleteActivity)this;
    public MessageReactionActivity ToMessageReaction() => (MessageReactionActivity)this;
    public ConversationUpdateActivity ToConversationUpdate() => (ConversationUpdateActivity)this;
    public EndOfConversationActivity ToEndOfConversation() => (EndOfConversationActivity)this;
    public EventActivity ToEvent() => (EventActivity)this;
    public InvokeActivity ToInvoke() => (InvokeActivity)this;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}