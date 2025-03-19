using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Invoke = new("invoke");
    public bool IsInvoke => Invoke.Equals(Value);
}

[JsonConverter(typeof(JsonConverter<InvokeName>))]
public partial class InvokeName(string value) : StringEnum(value)
{
}

public class InvokeActivity(InvokeName name) : Activity(ActivityType.Invoke)
{
    /// <summary>
    /// The name of the operation associated with an invoke or event activity.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(31)]
    public InvokeName Name { get; set; } = name;

    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public object? Value { get; set; }
}