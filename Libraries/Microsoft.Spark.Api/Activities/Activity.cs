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

[JsonConverter(typeof(JsonConverter))]
public partial interface IActivity : IConvertible, ICloneable
{
    [AllowNull]
    public string Id { get; set; }

    public ActivityType Type { get; init; }

    public string? ReplyToId { get; set; }

    public ChannelId ChannelId { get; set; }

    [AllowNull]
    public Account From { get; set; }

    [AllowNull]
    public Account Recipient { get; set; }

    [AllowNull]
    public Conversation Conversation { get; set; }

    public ConversationReference? RelatesTo { get; set; }

    public string? ServiceUrl { get; set; }

    public string? Locale { get; set; }

    public DateTime? Timestamp { get; set; }

    public DateTime? LocalTimestamp { get; set; }

    public IList<IEntity>? Entities { get; set; }

    public ChannelData? ChannelData { get; set; }

    public IDictionary<string, object?> Properties { get; set; }

    /// <summary>
    /// is this a streaming activity
    /// </summary>
    [JsonIgnore]
    public bool IsStreaming { get; }

    public string GetPath();
}

[JsonConverter(typeof(JsonConverter))]
public partial class Activity : IActivity
{
    [AllowNull]
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(10)]
    public ActivityType Type { get; init; }

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
    public IDictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();

    [JsonConstructor]
    public Activity(string type)
    {
        Type = new(type);
    }

    public Activity(ActivityType type)
    {
        Type = type;
    }

    public Activity(IActivity activity)
    {
        Id = activity.Id;
        Type = activity.Type;
        ReplyToId = activity.ReplyToId;
        ChannelId = activity.ChannelId;
        From = activity.From;
        Recipient = activity.Recipient;
        Conversation = activity.Conversation;
        RelatesTo = activity.RelatesTo;
        ServiceUrl = activity.ServiceUrl;
        Locale = activity.Locale;
        Timestamp = activity.Timestamp;
        LocalTimestamp = activity.LocalTimestamp;
        Entities = activity.Entities;
        ChannelData = activity.ChannelData;
        Properties = activity.Properties;
    }

    /// <summary>
    /// is this a streaming activity
    /// </summary>
    [JsonIgnore]
    public bool IsStreaming => Entities?.Any(entity => entity.Type == "streaminfo" && entity is StreamInfoEntity) ?? false;

    public object Clone() => MemberwiseClone();
    public virtual Activity Copy() => (Activity)Clone();
    public virtual string GetPath() => string.Join('.', ["Activity", Type.ToPrettyString()]);

    public virtual Activity WithId(string value)
    {
        Id = value;
        return this;
    }

    public virtual Activity WithReplyToId(string value)
    {
        ReplyToId = value;
        return this;
    }

    public virtual Activity WithChannelId(ChannelId value)
    {
        ChannelId = value;
        return this;
    }

    public virtual Activity WithFrom(Account value)
    {
        From = value;
        return this;
    }

    public virtual Activity WithConversation(Conversation value)
    {
        Conversation = value;
        return this;
    }

    public virtual Activity WithRelatesTo(ConversationReference value)
    {
        RelatesTo = value;
        return this;
    }

    public virtual Activity WithRecipient(Account value)
    {
        Recipient = value;
        return this;
    }

    public virtual Activity WithServiceUrl(string value)
    {
        ServiceUrl = value;
        return this;
    }

    public virtual Activity WithTimestamp(DateTime value)
    {
        Timestamp = value;
        return this;
    }

    public virtual Activity WithLocale(string value)
    {
        Locale = value;
        return this;
    }

    public virtual Activity WithLocalTimestamp(DateTime value)
    {
        LocalTimestamp = value;
        return this;
    }

    public virtual Activity WithData(ChannelData value)
    {
        ChannelData = value;
        return this;
    }

    public virtual Activity WithData(string key, object? value)
    {
        ChannelData ??= new ChannelData();
        ChannelData.Properties[key] = value;
        return this;
    }

    public virtual Activity WithAppId(string value)
    {
        ChannelData ??= new ChannelData();
        ChannelData.App ??= new App() { Id = value };
        return this;
    }

    /// <summary>
    /// add an entity
    /// </summary>
    public virtual Activity AddEntity(params IEntity[] entities)
    {
        Entities ??= [];

        foreach (var entity in entities)
        {
            Entities.Add(entity);
        }

        return this;
    }

    /// <summary>
    /// add the `Generated By AI` label
    /// </summary>
    public virtual Activity AddAIGenerated()
    {
        return AddEntity(new MessageEntity()
        {
            Type = "https://schema.org/Message",
            OType = "Message",
            OContext = "https://schema.org",
            AdditionalType = ["AIGeneratedContent"]
        });
    }

    /// <summary>
    /// add content sensitivity label
    /// </summary>
    /// <param name="name">the content title</param>
    /// <param name="description">the content description</param>
    /// <param name="pattern">the pattern</param>
    public virtual Activity AddSensitivityLabel(string name, string? description = null, DefinedTerm? pattern = null)
    {
        return AddEntity(new SensitiveUsageEntity()
        {
            Name = name,
            Description = description,
            Pattern = pattern
        });
    }

    /// <summary>
    /// enable/disable message feedback
    /// </summary>
    public virtual Activity AddFeedback(bool value = true)
    {
        ChannelData ??= new ChannelData();
        ChannelData.FeedbackLoopEnabled = value;
        return this;
    }

    /// <summary>
    /// add a citation
    /// </summary>
    public virtual Activity AddCitation(int position, CitationAppearance appearance)
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