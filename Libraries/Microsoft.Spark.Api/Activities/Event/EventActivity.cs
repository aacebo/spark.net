using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Event = new("event");
    public bool IsEvent => Event.Equals(Value);
}

[JsonConverter(typeof(JsonConverter<EventName>))]
public partial class EventName(string value) : StringEnum(value)
{
}

public class EventActivity(EventName name) : Activity(ActivityType.Event)
{
    /// <summary>
    /// The name of the operation associated with an invoke or event activity.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(31)]
    public EventName Name { get; set; } = name;
}