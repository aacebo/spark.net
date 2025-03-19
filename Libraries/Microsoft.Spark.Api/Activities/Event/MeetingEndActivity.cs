using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class EventName : StringEnum
{
    public static readonly EventName MeetingEnd = new("application/vnd.microsoft.meetingEnd");
    public bool IsMeetingEnd => MeetingEnd.Equals(Value);
}

public class MeetingEndActivity() : EventActivity(EventName.MeetingEnd)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public required MeetingEndActivityValue Value { get; set; }
}

/// <summary>
/// A value that is associated with the activity.
/// </summary>
public class MeetingEndActivityValue
{
    /// <summary>
    /// The meeting's Id, encoded as a BASE64 string.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    /// <summary>
    /// The meeting's type.
    /// </summary>
    [JsonPropertyName("meetingType")]
    [JsonPropertyOrder(1)]
    public required string MeetingType { get; set; }

    /// <summary>
    /// The URL used to join the meeting.
    /// </summary>
    [JsonPropertyName("joinUrl")]
    [JsonPropertyOrder(2)]
    public required string JoinUrl { get; set; }

    /// <summary>
    /// The title of the meeting.
    /// </summary>
    [JsonPropertyName("title")]
    [JsonPropertyOrder(3)]
    public required string Title { get; set; }

    /// <summary>
    /// Timestamp for meeting end, in UTC.
    /// </summary>
    [JsonPropertyName("endTime")]
    [JsonPropertyOrder(4)]
    public required DateTime EndTime { get; set; }
}