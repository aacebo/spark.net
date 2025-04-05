using System.Text.Json;

using Json.Schema;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public static partial class Extensions
{
    public static IFunction ToSpark(this ChatTool tool)
    {
        return new Function(
            tool.FunctionName,
            tool.FunctionDescription,
            JsonSchema.FromText(tool.FunctionParameters.ToString()),
            (_) => Task.FromResult<object?>(null)
        );
    }

    public static ChatTool ToOpenAI(this IFunction function)
    {
        return ChatTool.CreateFunctionTool(
            function.Name,
            function.Description,
            function.Parameters == null ? BinaryData.Empty : BinaryData.FromString(JsonSerializer.Serialize(function.Parameters))
        );
    }
}