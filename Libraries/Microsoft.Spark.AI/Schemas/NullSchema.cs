using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

public static partial class Schemas
{
    /// <summary>
    /// a schema that validates null values
    /// </summary>   
    public static NullSchema Null() => new();
}

/// <summary>
/// a schema that validates null values
/// </summary>
public class NullSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Null;

    public override NullSchema WithSchema(string value) => (NullSchema)base.WithSchema(value);
    public override NullSchema WithRef(string value) => (NullSchema)base.WithRef(value);
    public override NullSchema WithId(string value) => (NullSchema)base.WithId(value);
    public override NullSchema WithTitle(string value) => (NullSchema)base.WithTitle(value);
    public override NullSchema WithDescription(string value) => (NullSchema)base.WithDescription(value);
}