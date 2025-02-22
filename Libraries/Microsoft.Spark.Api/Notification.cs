using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class Notification
{
    [JsonPropertyName("alert")]
    [JsonPropertyOrder(0)]
    public bool? Alert { get; set; }

    [JsonPropertyName("alertInMeeting")]
    [JsonPropertyOrder(1)]
    public bool? AlertInMeeting { get; set; }

    [JsonPropertyName("externalResourceUrl")]
    [JsonPropertyOrder(2)]
    public string? ExternalResourceUrl { get; set; }
}