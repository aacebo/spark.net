using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static ModelMessage<string> ToSpark(this AssistantChatMessage message)
    {
        var calls = message.ToolCalls.Select(call =>
        {
            var args = call.FunctionArguments.ToString();
            return new FunctionCall()
            {
                Id = call.Id,
                Name = call.FunctionName,
                Arguments = args == string.Empty ? null : args
            };
        });

        return new ModelMessage<string>(message.Content.First().Text, calls.ToList());
    }

    public static AssistantChatMessage ToOpenAI(this ModelMessage<string> message)
    {
        var calls = message.FunctionCalls?.Select(call => ChatToolCall.CreateFunctionToolCall(
            call.Id,
            call.Name,
            call.Arguments == null ? BinaryData.Empty : BinaryData.FromString(call.Arguments)
        ));

        if (calls != null && calls.Count() > 0)
        {
            return new AssistantChatMessage(calls?.ToList() ?? []);
        }

        return new AssistantChatMessage(message.Content);
    }
}