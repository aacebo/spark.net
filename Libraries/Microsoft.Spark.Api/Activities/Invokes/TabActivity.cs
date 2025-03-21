using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Tab Activity
/// </summary>
[JsonConverter(typeof(TabActivityJsonConverter))]
public abstract class TabActivity(Name.Tabs name) : InvokeActivity(new(name.Value))
{
    public Tabs.FetchActivity ToFetch() => (Tabs.FetchActivity)this;
    public Tabs.SubmitActivity ToSubmit() => (Tabs.SubmitActivity)this;
}

public class TabActivityJsonConverter : JsonConverter<TabActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override TabActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "tab/fetch" => JsonSerializer.Deserialize<Tabs.FetchActivity>(element.ToString(), options),
            "tab/submit" => JsonSerializer.Deserialize<Tabs.SubmitActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<TabActivity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, TabActivity value, JsonSerializerOptions options)
    {
        if (value is Tabs.FetchActivity fetch)
        {
            JsonSerializer.Serialize(writer, fetch, options);
            return;
        }

        if (value is Tabs.SubmitActivity submit)
        {
            JsonSerializer.Serialize(writer, submit, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}