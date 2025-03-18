using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Events;

public class ActivityResponseEventArgs : ConversationReference
{
    public required IActivity Activity { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityResponseEventAttribute() : EventAttribute("activity.response", typeof(ActivityResponseEventArgs))
{
}