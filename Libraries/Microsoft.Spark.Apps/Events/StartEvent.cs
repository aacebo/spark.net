namespace Microsoft.Spark.Apps.Events;

public class StartEventArgs : EventArgs
{
    public required Common.Logging.ILogger Logger { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class StartEventAttribute() : EventAttribute("start", typeof(StartEventArgs))
{
}