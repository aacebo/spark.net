namespace Microsoft.Spark.Apps.Events;

public class ErrorEventArgs : EventArgs
{
    public required Exception Error { get; set; }
    public required Common.Logging.ILogger Logger { get; set; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class @ErrorEventAttribute() : EventAttribute("error", typeof(ErrorEventArgs))
{
}