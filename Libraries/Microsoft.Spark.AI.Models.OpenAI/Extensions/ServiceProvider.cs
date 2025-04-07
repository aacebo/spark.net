using Microsoft.Spark.AI.Prompts;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static IChatPrompt<ChatCompletionOptions> GetOpenAIChatPrompt(this IServiceProvider provider)
    {
        var prompt = provider.GetService(typeof(IChatPrompt<ChatCompletionOptions>)) ?? throw new Exception("chat prompt not found");
        return (IChatPrompt<ChatCompletionOptions>)prompt;
    }
}