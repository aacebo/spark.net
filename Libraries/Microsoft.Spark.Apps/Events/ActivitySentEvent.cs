using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Events;

public class ActivitySentEventArgs : ConversationReference
{
    public required IActivity Activity { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivitySentEventAttribute() : EventAttribute("activity.sent", typeof(ActivitySentEventArgs))
{
}