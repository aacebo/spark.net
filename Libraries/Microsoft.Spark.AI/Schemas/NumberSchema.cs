using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates number values
    /// </summary>
    public static NumberSchema Number() => new();
}

/// <summary>
/// a schema that validates number values
/// </summary>
public class NumberSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Number;

    [JsonPropertyName("enum")]
    [JsonPropertyOrder(6)]
    public new float?[]? Enum { get; set; }

    [JsonPropertyName("min")]
    [JsonPropertyOrder(7)]
    public float? Min { get; set; }

    [JsonPropertyName("max")]
    [JsonPropertyOrder(8)]
    public float? Max { get; set; }

    [JsonPropertyName("multipleOf")]
    [JsonPropertyOrder(9)]
    public float? MultipleOf { get; set; }

    public override NumberSchema WithSchema(string value) => (NumberSchema)base.WithSchema(value);
    public override NumberSchema WithRef(string value) => (NumberSchema)base.WithRef(value);
    public override NumberSchema WithId(string value) => (NumberSchema)base.WithId(value);
    public override NumberSchema WithTitle(string value) => (NumberSchema)base.WithTitle(value);
    public override NumberSchema WithDescription(string value) => (NumberSchema)base.WithDescription(value);
    public NumberSchema WithEnum(params float?[] value) => (NumberSchema)base.WithEnum(value);

    public NumberSchema WithMin(float value)
    {
        Min = value;
        return this;
    }

    public NumberSchema WithMax(float value)
    {
        Max = value;
        return this;
    }

    public NumberSchema WithMultipleOf(float value)
    {
        MultipleOf = value;
        return this;
    }
}