using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

[JsonConverter(typeof(JsonConverter<AdaptiveCardActionType>))]
public partial class AdaptiveCardActionType(string value) : StringEnum(value)
{
    public static readonly AdaptiveCardActionType Execute = new("Action.Execute");
    public bool IsExecute => Execute.Equals(Value);

    public static readonly AdaptiveCardActionType Submit = new("Action.Submit");
    public bool IsSubmit => Submit.Equals(Value);
}