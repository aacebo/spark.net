using Microsoft.Extensions.Configuration;

namespace Microsoft.Spark.AspNetCore;

public static class ConfigurationExtensions
{
    public static SparkSettings? GetSpark(this IConfiguration configuration)
    {
        return configuration.GetSection("Spark").Get<SparkSettings>();
    }
}