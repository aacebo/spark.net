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

    public Events.MeetingStartActivity ToMeetingStart() => (Events.MeetingStartActivity)this;
    public Events.MeetingEndActivity ToMeetingEnd() => (Events.MeetingEndActivity)this;
    public Events.MeetingParticipantJoinActivity ToMeetingParticipantJoin() => (Events.MeetingParticipantJoinActivity)this;
    public Events.MeetingParticipantLeaveActivity ToMeetingParticipantLeave() => (Events.MeetingParticipantLeaveActivity)this;
    public Events.ReadReceiptActivity ToReadReceipt() => (Events.ReadReceiptActivity)this;
}