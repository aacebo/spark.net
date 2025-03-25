using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Events;

/// <summary>
/// the event emitted by a plugin before an invoke response is returned
/// </summary>
public class ActivityResponseEventArgs : ConversationReference
{
    /// <summary>
    /// the inbound request activity payload
    /// </summary>
    public required IActivity Activity { get; set; }

    /// <summary>
    /// the response
    /// </summary>
    public required Response Response { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityResponseEventAttribute() : EventAttribute("activity.response", typeof(ActivityResponseEventArgs))
{
}