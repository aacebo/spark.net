using System.Text.Json.Serialization;

using Microsoft.Spark.Api.Entities;
using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType MessageUpdate = new("messageUpdate");
    public bool IsMessageUpdate => MessageUpdate.Equals(Value);
}

public class MessageUpdateActivity : Activity
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

    public MessageUpdateActivity() : base(ActivityType.MessageUpdate)
    {
    }

    public MessageUpdateActivity(string text) : base(ActivityType.MessageUpdate)
    {
        Text = text;
    }

    public MessageUpdateActivity Mention(Account account)
    {
        return (MessageUpdateActivity)AddEntity(new MentionEntity()
        {
            Mentioned = account,
            Text = $"<at>{account.Name}</at>"
        });
    }
}