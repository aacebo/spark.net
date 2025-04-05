using System.Text.Json;

using Json.Schema;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static IFunction ToSpark(this ChatTool tool)
    {
        var parameters = tool.FunctionParameters.ToString();

        return new Function(
            tool.FunctionName,
            tool.FunctionDescription,
            JsonSchema.FromText(parameters == string.Empty ? "{}" : parameters),
            (_) => Task.FromResult<object?>(null)
        );
    }

    public static ChatTool ToOpenAI(this IFunction function)
    {
        return ChatTool.CreateFunctionTool(
            function.Name,
            function.Description,
            function.Parameters == null ? null : BinaryData.FromString(JsonSerializer.Serialize(function.Parameters)),
            false
        );
    }
}