using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Agents;

[TrueTypeJson<IMessage>]
public interface IMessage
{
    public IHeaders Headers { get; }
    public string Type { get; }
    public object Content { get; }
}