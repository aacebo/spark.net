using System.Diagnostics.CodeAnalysis;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;

namespace Microsoft.Spark.Apps.Extensions;

public class SparkContext
{
    [AllowNull]
    public IToken Token { get; set; }

    [AllowNull]
    public IContext<IActivity> Activity { get; set; }

    public Response? Response { get; set; }
}