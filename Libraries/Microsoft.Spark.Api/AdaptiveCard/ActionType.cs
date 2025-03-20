using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.AdaptiveCard;

[JsonConverter(typeof(JsonConverter<ActionType>))]
public partial class ActionType(string value) : StringEnum(value)
{
    public static readonly ActionType Execute = new("Action.Execute");
    public bool IsExecute => Execute.Equals(Value);

    public static readonly ActionType Submit = new("Action.Submit");
    public bool IsSubmit => Submit.Equals(Value);
}