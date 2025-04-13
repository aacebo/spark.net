using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Agents.A2A.Models;

[TrueTypeJson<IPart>]
public interface IPart
{
    public string Type { get; }
    public IDictionary<string, object>? MetaData { get; }
}