using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates a collection of values
    /// </summary>
    public static ArraySchema Array(ISchema items) => new(items);

    /// <summary>
    /// a schema that validates a collection of values
    /// </summary>
    public static ArraySchema Array(params ISchema[] items) => new(items);
}

/// <summary>
/// a schema that validates a collection of values
/// </summary>
public class ArraySchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Array;

    [JsonPropertyName("items")]
    [JsonPropertyOrder(6)]
    public object Items { get; set; }

    [JsonPropertyName("additionalItems")]
    [JsonPropertyOrder(7)]
    public object? AdditionalItems { get; set; }

    public ArraySchema(ISchema items)
    {
        Items = items;
    }

    public ArraySchema(params ISchema[] items)
    {
        Items = items;
    }

    public override ArraySchema WithSchema(string value) => (ArraySchema)base.WithSchema(value);
    public override ArraySchema WithRef(string value) => (ArraySchema)base.WithRef(value);
    public override ArraySchema WithId(string value) => (ArraySchema)base.WithId(value);
    public override ArraySchema WithTitle(string value) => (ArraySchema)base.WithTitle(value);
    public override ArraySchema WithDescription(string value) => (ArraySchema)base.WithDescription(value);

    public ArraySchema WithAdditionalItems(ISchema value)
    {
        AdditionalItems = value;
        return this;
    }

    public ArraySchema WithAdditionalItems(params ISchema[] value)
    {
        AdditionalItems = value;
        return this;
    }
}