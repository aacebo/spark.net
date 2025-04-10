using Microsoft.Extensions.Configuration;
using Microsoft.Spark.AI.Models.OpenAI;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Extensions.Configuration;

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

    public static OpenAISettings GetOpenAI(this IConfigurationManager manager)
    {
        return manager.GetRequiredSection("OpenAI").Get<OpenAISettings>() ?? throw new Exception("OpenAI Configuration Not Found");
    }
}