using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Event = new("event");
    public bool IsEvent => Event.Equals(Value);
}

public class EventActivity(Events.Name name) : Activity(ActivityType.Event)
{
    /// <summary>
    /// The name of the operation associated with an invoke or event activity.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(31)]
    public Events.Name Name { get; set; } = name;
}