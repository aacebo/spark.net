using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema with no particular type
    /// </summary>
    public static AnySchema Any() => new();
}

/// <summary>
/// a schema with no particular type
/// </summary>
public class AnySchema : ISchema
{
    [JsonPropertyName("$schema")]
    [JsonPropertyOrder(0)]
    public string? Schema { get; set; }

    [JsonPropertyName("$ref")]
    [JsonPropertyOrder(1)]
    public string? Ref { get; set; }

    [JsonPropertyName("id")]
    [JsonPropertyOrder(2)]
    public string? Id { get; set; }

    [JsonPropertyName("title")]
    [JsonPropertyOrder(3)]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    [JsonPropertyOrder(4)]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public virtual SchemaType? Type => null;

    [JsonPropertyName("enum")]
    [JsonPropertyOrder(6)]
    public object?[]? Enum { get; set; }

    public virtual AnySchema WithSchema(string value)
    {
        Schema = value;
        return this;
    }

    public virtual AnySchema WithRef(string value)
    {
        Ref = value;
        return this;
    }

    public virtual AnySchema WithId(string value)
    {
        Id = value;
        return this;
    }

    public virtual AnySchema WithTitle(string value)
    {
        Title = value;
        return this;
    }

    public virtual AnySchema WithDescription(string value)
    {
        Description = value;
        return this;
    }

    public virtual AnySchema WithEnum(params object?[] value)
    {
        Enum = value;
        return this;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }
}