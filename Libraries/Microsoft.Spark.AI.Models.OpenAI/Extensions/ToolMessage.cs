using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static FunctionMessage ToSpark(this ToolChatMessage message)
    {
        return new FunctionMessage()
        {
            FunctionId = message.ToolCallId,
            Content = message.Content.FirstOrDefault()?.Text
        };
    }

    public static ToolChatMessage ToOpenAI(this FunctionMessage message)
    {
        return ChatMessage.CreateToolMessage(
            message.FunctionId,
            message.Content ?? string.Empty
        );
    }
}