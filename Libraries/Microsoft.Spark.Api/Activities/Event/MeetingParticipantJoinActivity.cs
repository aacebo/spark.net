using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class EventName : StringEnum
{
    public static readonly EventName MeetingParticipantJoin = new("application/vnd.microsoft.meetingParticipantJoin");
    public bool IsMeetingParticipantJoin => MeetingParticipantJoin.Equals(Value);
}

public class MeetingParticipantJoinActivity() : EventActivity(EventName.MeetingParticipantJoin)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public required MeetingParticipantJoinActivityValue Value { get; set; }
}

/// <summary>
/// A value that is associated with the activity.
/// </summary>
public class MeetingParticipantJoinActivityValue
{
    /// <summary>
    /// The participants info.
    /// </summary>
    [JsonPropertyName("members")]
    [JsonPropertyOrder(0)]
    public required IList<Member> Members { get; set; }

    public class Member
    {
        /// <summary>
        /// The participant account.
        /// </summary>
        [JsonPropertyName("user")]
        [JsonPropertyOrder(0)]
        public required Account User { get; set; }

        /// <summary>
        /// The participants info.
        /// </summary>
        [JsonPropertyName("meeting")]
        [JsonPropertyOrder(1)]
        public required Meeting Meeting { get; set; }
    }

    public class Meeting
    {
        /// <summary>
        /// The user in meeting indicator.
        /// </summary>
        [JsonPropertyName("inMeeting")]
        [JsonPropertyOrder(0)]
        public required bool InMeeting { get; set; }

        [JsonPropertyName("role")]
        [JsonPropertyOrder(1)]
        public required Role Role { get; set; }
    }
}