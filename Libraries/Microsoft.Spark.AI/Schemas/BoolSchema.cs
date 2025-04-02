using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates bool values
    /// </summary>   
    public static BoolSchema Bool() => new();
}

/// <summary>
/// a schema that validates bool values
/// </summary>
public class BoolSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Bool;

    [JsonPropertyName("enum")]
    [JsonPropertyOrder(6)]
    public bool?[]? Enum { get; set; }

    public override BoolSchema WithSchema(string value) => (BoolSchema)base.WithSchema(value);
    public override BoolSchema WithRef(string value) => (BoolSchema)base.WithRef(value);
    public override BoolSchema WithId(string value) => (BoolSchema)base.WithId(value);
    public override BoolSchema WithTitle(string value) => (BoolSchema)base.WithTitle(value);
    public override BoolSchema WithDescription(string value) => (BoolSchema)base.WithDescription(value);

    public BoolSchema WithEnum(params bool?[] value)
    {
        Enum = value;
        return this;
    }
}