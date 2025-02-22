using Microsoft.AspNetCore.Builder;

namespace Microsoft.Spark.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSpark(this IApplicationBuilder builder)
    {
        return builder;
    }
}