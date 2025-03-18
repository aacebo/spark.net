using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;

namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageActivity : IMessageActivityBase
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
}

public class MessageActivity : MessageActivityBase, IMessageActivity
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

    public MessageActivity(string text) : base()
    {
        Type = "message";
        Text = text;
    }

    public MessageActivity Attachment(Attachment value)
    {
        if (Attachments == null)
        {
            Attachments = [];
        }

        Attachments.Add(value);
        return this;
    }

    public MessageActivity Mention(Account account)
    {
        return (MessageActivity)Entity(new MentionEntity()
        {
            Mentioned = account,
            Text = $"<at>{account.Name}</at>"
        });
    }
}