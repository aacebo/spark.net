using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Api.Auth;
using Microsoft.Spark.Apps;

namespace Microsoft.Spark.Plugins.AspNetCore;

public class SparkHttpContext
{
    [AllowNull]
    public IToken Token { get; set; }

    [AllowNull]
    public IContext<IActivity> Activity { get; set; }

    public Response? Response { get; set; }
}

public static class ServiceProviderExtensions
{
    public static IContext<IActivity> GetSparkContext(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IContext<IActivity>>();
    }

    public static IActivity GetSparkActivity(this IServiceProvider provider)
    {
        return provider.GetRequiredService<IContext<IActivity>>().Activity;
    }
}