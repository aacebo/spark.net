using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class MessageExtensions
{
    public static IMessage ToSpark(this ChatMessage message)
    {
        if (message is SystemChatMessage system) return system.ToSpark();
        if (message is AssistantChatMessage assistant) return assistant.ToSpark();
        if (message is ToolChatMessage tool) return tool.ToSpark();
        if (message is UserChatMessage user) return user.ToSpark();
        throw new Exception("OpenAI ChatMessage type not supported");
    }

    public static ChatMessage ToOpenAI(this IMessage message)
    {
        if (message is DeveloperMessage developer) return developer.ToOpenAI();
        if (message is ModelMessage<string> model) return model.ToOpenAI();
        if (message is FunctionMessage function) return function.ToOpenAI();
        if (message is UserMessage<string> userText) return userText.ToOpenAI();
        if (message is UserMessage<IEnumerable<IContent>> userParts) return userParts.ToOpenAI();
        throw new Exception("Spark Message type not supported");
    }
}