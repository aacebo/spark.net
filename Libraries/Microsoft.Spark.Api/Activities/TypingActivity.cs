using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities;

public class TypingActivity : Activity
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string? Text { get; set; }

    public TypingActivity() : base("typing")
    {
    }

    public TypingActivity(string text) : base("typing")
    {
        Text = text;
    }
}