using Microsoft.Extensions.Configuration;

namespace Microsoft.Spark.Plugins.AspNetCore.DevTools.Extensions;

public static class ConfigurationManagerExtensions
{
    public static SparkDevToolsSettings GetSparkDevTools(this IConfigurationManager manager)
    {
        return manager.GetSection("Spark").GetSection("Plugins.DevTools").Get<SparkDevToolsSettings>() ?? new();
    }
}