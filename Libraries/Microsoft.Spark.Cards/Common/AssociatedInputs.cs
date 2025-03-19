using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Cards;

[JsonConverter(typeof(JsonConverter<AssociatedInputs>))]
public partial class AssociatedInputs(string value) : StringEnum(value, caseSensitive: false)
{
    public static readonly AssociatedInputs Auto = new("auto");
    public bool IsAuto => Auto.Equals(Value);

    public static readonly AssociatedInputs None = new("none");
    public bool IsNone => None.Equals(Value);
}