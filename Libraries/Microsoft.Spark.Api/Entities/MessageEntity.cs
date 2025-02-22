using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Entities;

[JsonConverter(typeof(MessageEntityJsonConverter))]
public interface IMessageEntity : IEntity
{
    [JsonPropertyName("additionalType")]
    [JsonPropertyOrder(10)]
    public IList<string>? AdditionalType { get; set; }
}

public class MessageEntity : Entity, IMessageEntity
{
    [JsonPropertyName("additionalType")]
    [JsonPropertyOrder(10)]
    public IList<string>? AdditionalType { get; set; }

    public MessageEntity() : base("message") { }
}

public class OMessageEntity : Entity, IMessageEntity
{
    [JsonPropertyName("additionalType")]
    [JsonPropertyOrder(10)]
    public IList<string>? AdditionalType { get; set; }

    public OMessageEntity() : base("https://schema.org/Message")
    {
        OType = "Message";
        OContext = "https://schema.org";
    }
}

public class MessageEntityJsonConverter : JsonConverter<IMessageEntity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override IMessageEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (!element.TryGetProperty("type", out JsonElement property))
        {
            throw new JsonException("entity must have a 'type' property");
        }

        var type = property.Deserialize<string>(options);

        if (type == null)
        {
            throw new JsonException("failed to deserialize entity 'type' property");
        }

        return type switch
        {
            "message" => JsonSerializer.Deserialize<MessageEntity>(element.ToString(), options),
            "https://schema.org/Message" => ReadOMessage(element, options),
            _ => throw new JsonException($"entity type '{type}' is not supported")
        };
    }

    public override void Write(Utf8JsonWriter writer, IMessageEntity value, JsonSerializerOptions options)
    {
        if (value is ICitationEntity citation)
        {
            JsonSerializer.Serialize(writer, citation, options);
            return;
        }

        if (value is ISensitiveUsageEntity sensitiveUsage)
        {
            JsonSerializer.Serialize(writer, sensitiveUsage, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }

    protected IMessageEntity? ReadOMessage(JsonElement element, JsonSerializerOptions options)
    {
        if (!element.TryGetProperty("@type", out JsonElement property))
        {
            throw new JsonException("'https://schema.org/Message' entity must have a '@type' property");
        }

        var oType = property.Deserialize<string>(options);

        if (oType == null)
        {
            throw new JsonException("failed to deserialize 'https://schema.org/Message' entity '@type' property");
        }

        return oType switch
        {
            "Claim" => JsonSerializer.Deserialize<CitationEntity>(element.ToString(), options),
            "CreativeWork" => JsonSerializer.Deserialize<SensitiveUsageEntity>(element.ToString(), options),
            "Message" => JsonSerializer.Deserialize<OMessageEntity>(element.ToString(), options),
            _ => throw new JsonException($"entity type '{oType}' is not supported")
        };
    }
}