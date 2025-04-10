using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Plugins.DevTools;

[TrueTypeJson<IEvent>]
public interface IEvent
{
    public Guid Id { get; }
    public string Type { get; }
    public object? Body { get; }
    public DateTime SentAt { get; }
}