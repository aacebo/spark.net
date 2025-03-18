using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;

namespace Microsoft.Spark.Apps.Events;

public class ActivityEventArgs : EventArgs
{
    public required IActivity Activity { get; set; }
    public required IToken Token { get; set; }
    public required Common.Logging.ILogger Logger { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityEventAttribute() : EventAttribute("activity", typeof(ActivityEventArgs))
{
}