using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.AI;

public interface ISchema
{
    /// <summary>
    /// the Json Schema type
    /// </summary>
    public SchemaType? Type { get; }
}

[JsonConverter(typeof(JsonConverter<SchemaType>))]
public class SchemaType(string value) : StringEnum(value)
{
    public static readonly SchemaType String = new("string");
    public bool IsString => String.Equals(Value);

    public static readonly SchemaType Number = new("number");
    public bool IsNumber => Number.Equals(Value);

    public static readonly SchemaType Bool = new("boolean");
    public bool IsBool => Bool.Equals(Value);

    public static readonly SchemaType Null = new("null");
    public bool IsNull => Null.Equals(Value);

    public static readonly SchemaType Integer = new("integer");
    public bool IsInteger => Integer.Equals(Value);

    public static readonly SchemaType Object = new("object");
    public bool IsObject => Object.Equals(Value);

    public static readonly SchemaType Array = new("array");
    public bool IsArray => Array.Equals(Value);
}