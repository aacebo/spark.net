using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Conversation;

public interface IConversationActivityBase : IActivity
{

}

public class ConversationActivityBase : Activity, IConversationActivityBase
{

}

public class ConversationActivityJsonConverter : JsonConverter<IConversationActivityBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override IConversationActivityBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "conversationUpdate" => JsonSerializer.Deserialize<ConversationUpdateActivity>(element.ToString(), options),
            "endOfConversation" => JsonSerializer.Deserialize<EndOfConversationActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<ConversationActivityBase>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, IConversationActivityBase value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}