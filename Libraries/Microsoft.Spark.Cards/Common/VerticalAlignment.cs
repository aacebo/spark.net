using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Cards;

/// <summary>
/// Describes how the image should be aligned if it must be cropped or if using repeat fill mode.
/// </summary>
[JsonConverter(typeof(JsonConverter<VerticalAlignment>))]
public partial class VerticalAlignment(string value) : StringEnum(value)
{
    public static readonly VerticalAlignment Top = new("top");
    public bool IsTop => Top.Equals(Value.ToLower());

    public static readonly VerticalAlignment Center = new("center");
    public bool IsCenter => Center.Equals(Value.ToLower());

    public static readonly VerticalAlignment Bottom = new("bottom");
    public bool IsBottom => Bottom.Equals(Value.ToLower());
}