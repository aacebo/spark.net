using Microsoft.Extensions.Configuration;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static class ConfigurationExtensions
{
    public static OpenAISettings GetOpenAI(this IConfiguration configuration)
    {
        return configuration.GetRequiredSection("OpenAI").Get<OpenAISettings>() ?? throw new Exception("OpenAI Configuration Not Found");
    }
}