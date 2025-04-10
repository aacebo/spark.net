using Microsoft.Spark.Common;
using Microsoft.Spark.Common.Json;

namespace Microsoft.Spark.Agents;

public partial class ContentType(string value) : StringEnum(value)
{
}

/// <summary>
/// some piece of content sent from one
/// agent to another
/// </summary>
[TrueTypeJson<IContent>]
public interface IContent
{
    /// <summary>
    /// the content type of the content
    /// </summary>
    public ContentType Type { get; }
}