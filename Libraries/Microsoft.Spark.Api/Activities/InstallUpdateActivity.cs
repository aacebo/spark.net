using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public class InstallUpdateActivity() : Activity("installationUpdate")
{
    [JsonPropertyName("action")]
    [JsonPropertyOrder(31)]
    public required InstallUpdateAction Action { get; set; }
}

[JsonConverter(typeof(JsonConverter<InstallUpdateAction>))]
public class InstallUpdateAction(string value) : StringEnum(value)
{
    public static readonly InstallUpdateAction Add = new("add");
    public bool IsAdd => Add.Equals(Value);

    public static readonly InstallUpdateAction Remove = new("remove");
    public bool IsRemove => Remove.Equals(Value);
}