using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Events;

/// <summary>
/// the event emitted by a plugin when an error occurs
/// </summary>
public class ErrorEventArgs : EventArgs
{
    /// <summary>
    /// the error
    /// </summary>
    public required Exception Error { get; set; }

    /// <summary>
    /// the ILogger instance
    /// </summary>
    public required Common.Logging.ILogger Logger { get; set; }

    /// <summary>
    /// the inbound request activity payload
    /// </summary>
    public IActivity? Activity { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ErrorEventAttribute() : EventAttribute("error", typeof(ErrorEventArgs))
{
}