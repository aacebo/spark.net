using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Meetings;

/// <summary>
/// General information about a Teams meeting.
/// </summary>
public class Meeting
{
    /// <summary>
    /// Unique identifier representing a meeting
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string? Id { get; set; }

    /// <summary>
    /// The specific details of a Teams meeting.
    /// </summary>
    [JsonPropertyName("details")]
    [JsonPropertyOrder(1)]
    public MeetingDetails? Details { get; set; }

    /// <summary>
    /// The Conversation Account for the meeting.
    /// </summary>
    [JsonPropertyName("conversation")]
    [JsonPropertyOrder(2)]
    public Conversation? Conversation { get; set; }

    /// <summary>
    /// The organizer's user information.
    /// </summary>
    [JsonPropertyName("organizer")]
    [JsonPropertyOrder(3)]
    public Account? Organizer { get; set; }
}