using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;

namespace Microsoft.Spark.Apps.Events;

/// <summary>
/// the event emitted by a plugin when an activity is received
/// </summary>
public class ActivityEventArgs : EventArgs
{
    /// <summary>
    /// the inbound request activity payload
    /// </summary>
    public required IActivity Activity { get; set; }

    /// <summary>
    /// the inbound request token
    /// </summary>
    public required IToken Token { get; set; }

    /// <summary>
    /// the ILogger instance
    /// </summary>
    public required Common.Logging.ILogger Logger { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityEventAttribute() : EventAttribute("activity", typeof(ActivityEventArgs))
{
}