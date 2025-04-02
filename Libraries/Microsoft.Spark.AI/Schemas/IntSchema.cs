using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates int values
    /// </summary>
    public static IntSchema Int() => new();
}

/// <summary>
/// a schema that validates int values
/// </summary>
public class IntSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Integer;

    [JsonPropertyName("enum")]
    [JsonPropertyOrder(6)]
    public new int?[]? Enum { get; set; }

    [JsonPropertyName("min")]
    [JsonPropertyOrder(7)]
    public int? Min { get; set; }

    [JsonPropertyName("max")]
    [JsonPropertyOrder(8)]
    public int? Max { get; set; }

    [JsonPropertyName("multipleOf")]
    [JsonPropertyOrder(9)]
    public int? MultipleOf { get; set; }

    public override IntSchema WithSchema(string value) => (IntSchema)base.WithSchema(value);
    public override IntSchema WithRef(string value) => (IntSchema)base.WithRef(value);
    public override IntSchema WithId(string value) => (IntSchema)base.WithId(value);
    public override IntSchema WithTitle(string value) => (IntSchema)base.WithTitle(value);
    public override IntSchema WithDescription(string value) => (IntSchema)base.WithDescription(value);
    public IntSchema WithEnum(params int?[] value) => (IntSchema)base.WithEnum(value);

    public IntSchema WithMin(int value)
    {
        Min = value;
        return this;
    }

    public IntSchema WithMax(int value)
    {
        Max = value;
        return this;
    }

    public IntSchema WithMultipleOf(int value)
    {
        MultipleOf = value;
        return this;
    }
}