using Microsoft.Spark.AI.Prompts;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public class OpenAIChatPrompt : ChatPrompt<ChatCompletionOptions>
{
    public OpenAIChatPrompt(OpenAIChatModel model, ChatPromptOptions? options = null) : base(model, options)
    {

    }

    public OpenAIChatPrompt(ChatPrompt<ChatCompletionOptions> prompt) : base(prompt)
    {

    }

    public static OpenAIChatPrompt From<T>(OpenAIChatModel model, T value, ChatPromptOptions? options = null) where T : class
    {
        return new OpenAIChatPrompt(ChatPrompt<ChatCompletionOptions>.From(model, value, options));
    }
}