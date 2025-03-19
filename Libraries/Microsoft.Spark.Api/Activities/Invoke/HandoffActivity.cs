using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class InvokeName : StringEnum
{
    public static readonly InvokeName Handoff = new("handoff/action");
    public bool IsHandoff => Handoff.Equals(Value);
}

public class HandoffActivity() : InvokeActivity(InvokeName.Handoff)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public new required HandoffActivityValue Value { get; set; }
}

public class HandoffActivityValue
{
    /// <summary>
    /// Continuation token used to get the conversation reference.
    /// </summary>
    [JsonPropertyName("continuation")]
    [JsonPropertyOrder(0)]
    public required string Continuation { get; set; }
}