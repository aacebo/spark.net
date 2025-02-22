using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities;

public interface ITypingActivity : IActivity
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string? Text { get; set; }
}

public class TypingActivity : Activity, ITypingActivity
{
    [JsonPropertyName("text")]
    [JsonPropertyOrder(31)]
    public string? Text { get; set; }

    public TypingActivity() : base()
    {
        Type = "typing";
    }

    public TypingActivity(string text) : base()
    {
        Type = "typing";
        Text = text;
    }
}