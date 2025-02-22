using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

[JsonConverter(typeof(JsonConverter<Action>))]
public class Action(string value) : StringEnum(value)
{
    public static readonly Action Accept = new("accept");
    public bool IsAccept => Accept.Equals(Value);

    public static readonly Action Decline = new("decline");
    public bool IsDecline => Decline.Equals(Value);
}