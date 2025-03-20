using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public static readonly Name AdaptiveCardAction = new("adaptiveCard/action");
    public bool IsAdaptiveCardAction => AdaptiveCardAction.Equals(Value);
}

public static partial class AdaptiveCards
{
    public class ActionActivity() : InvokeActivity(Name.AdaptiveCardAction)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.AdaptiveCards.InvokeValue Value { get; set; }
    }
}