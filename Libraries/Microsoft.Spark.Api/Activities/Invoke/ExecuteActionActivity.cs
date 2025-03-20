using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class InvokeName : StringEnum
{
    public static readonly InvokeName ExecuteAction = new("actionableMessage/executeAction");
    public bool IsExecuteAction => ExecuteAction.Equals(Value);
}

/// <summary>
/// The name of the operation associated with an invoke or event activity.
/// </summary>
public class ExecuteActionActivity(O365ConnectorCardActionQuery value) : InvokeActivity(InvokeName.ExecuteAction)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public new O365ConnectorCardActionQuery Value { get; set; } = value;
}