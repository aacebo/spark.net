using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates string values
    /// </summary>
    public static StringSchema String() => new();
}

/// <summary>
/// a schema that validates string values
/// </summary>
public class StringSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.String;

    [JsonPropertyName("enum")]
    [JsonPropertyOrder(6)]
    public string?[]? Enum { get; set; }

    [JsonPropertyName("pattern")]
    [JsonPropertyOrder(7)]
    public string? Pattern { get; set; }

    [JsonPropertyName("format")]
    [JsonPropertyOrder(8)]
    public string? Format { get; set; }

    [JsonPropertyName("minLength")]
    [JsonPropertyOrder(9)]
    public int? MinLength { get; set; }

    [JsonPropertyName("maxLength")]
    [JsonPropertyOrder(10)]
    public int? MaxLength { get; set; }

    public override StringSchema WithSchema(string value) => (StringSchema)base.WithSchema(value);
    public override StringSchema WithRef(string value) => (StringSchema)base.WithRef(value);
    public override StringSchema WithId(string value) => (StringSchema)base.WithId(value);
    public override StringSchema WithTitle(string value) => (StringSchema)base.WithTitle(value);
    public override StringSchema WithDescription(string value) => (StringSchema)base.WithDescription(value);

    public StringSchema WithEnum(params string?[] value)
    {
        Enum = value;
        return this;
    }

    public StringSchema WithPattern(string value)
    {
        Pattern = value;
        return this;
    }

    public StringSchema WithFormat(string value)
    {
        Format = value;
        return this;
    }

    public StringSchema WithMinLength(int value)
    {
        MinLength = value;
        return this;
    }

    public StringSchema WithMaxLength(int value)
    {
        MaxLength = value;
        return this;
    }
}