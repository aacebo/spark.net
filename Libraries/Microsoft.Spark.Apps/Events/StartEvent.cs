namespace Microsoft.Spark.Apps.Events;

/// <summary>
/// the event emitted by the app on startup
/// </summary>
public class StartEventArgs : EventArgs
{
    /// <summary>
    /// the ILogger instance
    /// </summary>
    public required Common.Logging.ILogger Logger { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class StartEventAttribute() : EventAttribute("start", typeof(StartEventArgs))
{
}