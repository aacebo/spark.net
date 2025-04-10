using Microsoft.Extensions.DependencyInjection;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Extensions;

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