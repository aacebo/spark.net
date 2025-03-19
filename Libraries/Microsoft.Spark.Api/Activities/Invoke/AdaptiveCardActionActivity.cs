using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class InvokeName : StringEnum
{
    public static readonly InvokeName AdaptiveCardAction = new("adaptiveCard/action");
    public bool IsAdaptiveCardAction => AdaptiveCardAction.Equals(Value);
}

public class AdaptiveCardActionActivity() : InvokeActivity(InvokeName.AdaptiveCardAction)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public new required AdaptiveCardInvokeValue Value { get; set; }
}