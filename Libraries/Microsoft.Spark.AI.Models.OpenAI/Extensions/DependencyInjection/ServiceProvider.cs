using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static class ServiceProviderExtensions
{
    public static OpenAIChatModel GetOpenAIChatModel(this IServiceProvider provider)
    {
        return provider.GetRequiredService<OpenAIChatModel>();
    }

    public static OpenAIChatPrompt GetOpenAIChatPrompt(this IServiceProvider provider)
    {
        return provider.GetRequiredService<OpenAIChatPrompt>();
    }
}