using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Events;

/// <summary>
/// the event emitted by a plugin when an activity is sent
/// </summary>
public class ActivitySentEventArgs : ConversationReference
{
    /// <summary>
    /// the sent activity
    /// </summary>
    public required IActivity Activity { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivitySentEventAttribute() : EventAttribute("activity.sent", typeof(ActivitySentEventArgs))
{
}