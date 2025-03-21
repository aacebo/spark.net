using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Config Activity
/// </summary>
[JsonConverter(typeof(ConfigActivityJsonConverter))]
public abstract class ConfigActivity(Name.Configs name) : InvokeActivity(new(name.Value))
{
    public Configs.FetchActivity ToFetch() => (Configs.FetchActivity)this;
    public Configs.SubmitActivity ToSubmit() => (Configs.SubmitActivity)this;
}

public class ConfigActivityJsonConverter : JsonConverter<ConfigActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override ConfigActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var element = JsonSerializer.Deserialize<JsonElement>(ref reader, options);

        if (!element.TryGetProperty("name", out JsonElement property))
        {
            throw new JsonException("invoke activity must have a 'name' property");
        }

        var name = property.Deserialize<string>(options);

        if (name == null)
        {
            throw new JsonException("failed to deserialize invoke activity 'name' property");
        }

        return name switch
        {
            "config/fetch" => JsonSerializer.Deserialize<Configs.FetchActivity>(element.ToString(), options),
            "config/submit" => JsonSerializer.Deserialize<Configs.SubmitActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<ConfigActivity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, ConfigActivity value, JsonSerializerOptions options)
    {
        if (value is Configs.FetchActivity fetch)
        {
            JsonSerializer.Serialize(writer, fetch, options);
            return;
        }

        if (value is Configs.SubmitActivity submit)
        {
            JsonSerializer.Serialize(writer, submit, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}