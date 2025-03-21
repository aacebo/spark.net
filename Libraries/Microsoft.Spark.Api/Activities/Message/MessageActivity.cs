using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;
using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Message = new("message");
    public bool IsMessage => Message.Equals(Value);
}

public class MessageActivity : Activity
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string Text { get; set; }

    [JsonPropertyName("speak")]
    [JsonPropertyOrder(32)]
    public string? Speak { get; set; }

    [JsonPropertyName("inputHint")]
    [JsonPropertyOrder(33)]
    public InputHint? InputHint { get; set; }

    [JsonPropertyName("summary")]
    [JsonPropertyOrder(34)]
    public string? Summary { get; set; }

    [JsonPropertyName("textFormat")]
    [JsonPropertyOrder(35)]
    public TextFormat? TextFormat { get; set; }

    [JsonPropertyName("attachmentLayout")]
    [JsonPropertyOrder(121)]
    public Attachment.Layout? AttachmentLayout { get; set; }

    [JsonPropertyName("attachments")]
    [JsonPropertyOrder(122)]
    public IList<Attachment>? Attachments { get; set; }

    [JsonPropertyName("suggestedActions")]
    [JsonPropertyOrder(123)]
    public SuggestedActions? SuggestedActions { get; set; }

    [JsonPropertyName("importance")]
    [JsonPropertyOrder(39)]
    public Importance? Importance { get; set; }

    [JsonPropertyName("deliveryMode")]
    [JsonPropertyOrder(41)]
    public DeliveryMode? DeliveryMode { get; set; }

    [JsonPropertyName("expiration")]
    [JsonPropertyOrder(42)]
    public DateTime? Expiration { get; set; }

    [JsonPropertyName("value")]
    [JsonPropertyOrder(43)]
    public dynamic? Value { get; set; }

    public MessageActivity() : base(ActivityType.Message)
    {
        Text = string.Empty;
    }

    public MessageActivity(string text) : base(ActivityType.Message)
    {
        Text = text;
    }

    public MessageActivity AddAttachment(Attachment value)
    {
        if (Attachments == null)
        {
            Attachments = [];
        }

        Attachments.Add(value);
        return this;
    }

    public MessageActivity AddAttachment(Spark.Cards.Card card)
    {
        return AddAttachment(new Attachment.AdaptiveCard(card));
    }

    public MessageActivity AddAttachment(Cards.OAuthCard card)
    {
        return AddAttachment(new Attachment.OAuthCard(card));
    }

    public MessageActivity AddAttachment(Cards.SignInCard card)
    {
        return AddAttachment(new Attachment.SignInCard(card));
    }

    public MessageActivity AddMention(Account account)
    {
        return (MessageActivity)AddEntity(new MentionEntity()
        {
            Mentioned = account,
            Text = $"<at>{account.Name}</at>"
        });
    }
}