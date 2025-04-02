using System.Text.Json.Serialization;

namespace Microsoft.Spark.AI;

/// <summary>
/// a schema that validates objects
/// </summary>
public class ObjectSchema : AnySchema
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(5)]
    public override SchemaType Type => SchemaType.Object;

    [JsonPropertyName("properties")]
    [JsonPropertyOrder(6)]
    public IDictionary<string, ISchema>? Properties { get; set; }

    [JsonPropertyName("required")]
    [JsonPropertyOrder(7)]
    public IList<string>? Required { get; set; }

    public ObjectSchema()
    {

    }

    public ObjectSchema(Dictionary<string, ISchema> properties)
    {
        Properties = properties;
    }

    public override ObjectSchema WithSchema(string value) => (ObjectSchema)base.WithSchema(value);
    public override ObjectSchema WithRef(string value) => (ObjectSchema)base.WithRef(value);
    public override ObjectSchema WithId(string value) => (ObjectSchema)base.WithId(value);
    public override ObjectSchema WithTitle(string value) => (ObjectSchema)base.WithTitle(value);
    public override ObjectSchema WithDescription(string value) => (ObjectSchema)base.WithDescription(value);

    public ObjectSchema Property(string key, ISchema schema, bool required = false)
    {
        Properties ??= new Dictionary<string, ISchema>();
        Properties[key] = schema;

        if (required == true)
        {
            Required ??= [];

            if (!Required.Contains(key))
            {
                Required.Add(key);
            }
        }

        return this;
    }
}