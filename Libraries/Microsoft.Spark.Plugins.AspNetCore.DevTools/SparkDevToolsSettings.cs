using Microsoft.Spark.Plugins.AspNetCore.DevTools.Models;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools.Extensions;

public class SparkDevToolsSettings
{
    public IList<Page> Pages { get; init; } = [];
}