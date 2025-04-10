using Microsoft.Extensions.Configuration;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps.Extensions;

public static class ConfigurationManagerExtensions
{
    public static SparkSettings GetSpark(this IConfigurationManager manager)
    {
        return manager.GetSection("Spark").Get<SparkSettings>() ?? new();
    }

    public static LoggingSettings GetSparkLogging(this IConfigurationManager manager)
    {
        return manager.GetSection("Logging").GetSection("Microsoft.Spark").Get<LoggingSettings>() ?? new();
    }
}