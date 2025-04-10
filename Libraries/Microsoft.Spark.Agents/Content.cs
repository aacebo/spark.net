using Microsoft.Spark.Common;
using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Agents;

public partial class ContentType(string value) : StringEnum(value)
{
}

[TrueTypeJson<IContent>]
public interface IContent
{
    public ContentType Type { get; }
}