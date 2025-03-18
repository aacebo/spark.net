using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;

namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageUpdateActivity : IMessageActivityBase
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string? Text { get; set; }

    [JsonPropertyName("speak")]
    [JsonPropertyOrder(32)]
    public string? Speak { get; set; }

    [JsonPropertyName("summary")]
    [JsonPropertyOrder(34)]
    public string? Summary { get; set; }

    [JsonPropertyName("expiration")]
    [JsonPropertyOrder(42)]
    public DateTime? Expiration { get; set; }

    [JsonPropertyName("value")]
    [JsonPropertyOrder(43)]
    public dynamic? Value { get; set; }
}

public class MessageUpdateActivity : MessageActivityBase, IMessageUpdateActivity
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string? Text { get; set; }

    [JsonPropertyName("speak")]
    [JsonPropertyOrder(32)]
    public string? Speak { get; set; }

    [JsonPropertyName("summary")]
    [JsonPropertyOrder(34)]
    public string? Summary { get; set; }

    [JsonPropertyName("expiration")]
    [JsonPropertyOrder(42)]
    public DateTime? Expiration { get; set; }

    [JsonPropertyName("value")]
    [JsonPropertyOrder(43)]
    public dynamic? Value { get; set; }

    public MessageUpdateActivity() : base()
    {
        Type = "messageUpdate";
    }

    public MessageUpdateActivity(string text) : base()
    {
        Type = "messageUpdate";
        Text = text;
    }

    public MessageUpdateActivity Mention(Account account)
    {
        return (MessageUpdateActivity)Entity(new MentionEntity()
        {
            Mentioned = account,
            Text = $"<at>{account.Name}</at>"
        });
    }
}