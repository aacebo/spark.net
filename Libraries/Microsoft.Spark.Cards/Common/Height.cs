using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Cards;

[JsonConverter(typeof(JsonConverter<Height>))]
public partial class Height(string value) : StringEnum(value, caseSensitive: false)
{
    public static readonly Height Auto = new("auto");
    public bool IsAuto => Auto.Equals(Value);

    public static readonly Height Stretch = new("stretch");
    public bool IsStretch => Stretch.Equals(Value);
}