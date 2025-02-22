using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Message;

[JsonConverter(typeof(MessageActivityJsonConverter))]
public interface IMessageActivity : IActivity
{

}

public class MessageActivity : Activity, IMessageActivity
{

}

public class MessageActivityJsonConverter : JsonConverter<IMessageActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override IMessageActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (!element.TryGetProperty("type", out JsonElement property))
        {
            throw new JsonException("activity must have a 'type' property");
        }

        var type = property.Deserialize<string>(options);

        if (type == null)
        {
            throw new JsonException("failed to deserialize activity 'type' property");
        }

        return type switch
        {
            "message" => JsonSerializer.Deserialize<MessageSendActivity>(element.ToString(), options),
            "messageUpdate" => JsonSerializer.Deserialize<MessageUpdateActivity>(element.ToString(), options),
            "messageDelete" => JsonSerializer.Deserialize<MessageDeleteActivity>(element.ToString(), options),
            "messageReaction" => JsonSerializer.Deserialize<MessageReactionActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<MessageActivity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, IMessageActivity value, JsonSerializerOptions options)
    {
        if (value is IMessageSendActivity send)
        {
            JsonSerializer.Serialize(writer, send, options);
            return;
        }

        if (value is IMessageUpdateActivity update)
        {
            JsonSerializer.Serialize(writer, update, options);
            return;
        }

        if (value is IMessageDeleteActivity delete)
        {
            JsonSerializer.Serialize(writer, delete, options);
            return;
        }

        if (value is IMessageReactionActivity reaction)
        {
            JsonSerializer.Serialize(writer, reaction, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}