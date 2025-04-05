using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static DeveloperMessage ToSpark(this SystemChatMessage message)
    {
        var content = message.Content.Select(c =>
        {
            if (c.Kind == ChatMessageContentPartKind.Text) return c.Text;
            if (c.Kind == ChatMessageContentPartKind.Image) return c.ImageUri.ToString();
            return c.Refusal;
        });

        return new DeveloperMessage(string.Join("\n", content));
    }

    public static SystemChatMessage ToOpenAI(this DeveloperMessage message)
    {
        return ChatMessage.CreateSystemMessage(message.Content);
    }
}