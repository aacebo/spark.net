using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Http;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.AspNetCore;

public class SparkContext
{
    [AllowNull]
    public HttpContext Http { get; internal set; }

    [AllowNull]
    public IToken Token { get; internal set; }

    [AllowNull]
    public IContext<IActivity> Activity { get; internal set; }

    public Response? Response { get; internal set; }
}