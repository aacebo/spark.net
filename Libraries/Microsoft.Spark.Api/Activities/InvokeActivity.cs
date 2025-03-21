using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Invoke = new("invoke");
    public bool IsInvoke => Invoke.Equals(Value);
}

[JsonConverter(typeof(InvokeActivityJsonConverter))]
public class InvokeActivity(Invokes.Name name) : Activity(ActivityType.Invoke)
{
    /// <summary>
    /// The name of the operation associated with an invoke or event activity.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(31)]
    public Invokes.Name Name { get; set; } = name;

    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public object? Value { get; set; }

    public Invokes.AdaptiveCardActivity ToAdaptiveCard() => (Invokes.AdaptiveCardActivity)this;
    public Invokes.ConfigActivity ToConfig() => (Invokes.ConfigActivity)this;
    public Invokes.MessageExtensionActivity ToMessageExtension() => (Invokes.MessageExtensionActivity)this;
    public new Invokes.MessageActivity ToMessage() => (Invokes.MessageActivity)this;
    public Invokes.SignInActivity ToSignIn() => (Invokes.SignInActivity)this;
    public Invokes.TabActivity ToTab() => (Invokes.TabActivity)this;
    public Invokes.TaskActivity ToTask() => (Invokes.TaskActivity)this;
}

public class InvokeActivityJsonConverter : JsonConverter<InvokeActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override InvokeActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

        if (name.StartsWith("adaptiveCard/"))
        {
            return JsonSerializer.Deserialize<Invokes.AdaptiveCardActivity>(element.ToString(), options);
        }

        if (name.StartsWith("config/"))
        {
            return JsonSerializer.Deserialize<Invokes.ConfigActivity>(element.ToString(), options);
        }

        if (name.StartsWith("composeExtension/"))
        {
            return JsonSerializer.Deserialize<Invokes.MessageExtensionActivity>(element.ToString(), options);
        }

        if (name.StartsWith("message/"))
        {
            return JsonSerializer.Deserialize<Invokes.MessageActivity>(element.ToString(), options);
        }

        if (name.StartsWith("signin/"))
        {
            return JsonSerializer.Deserialize<Invokes.SignInActivity>(element.ToString(), options);
        }

        if (name.StartsWith("tab/"))
        {
            return JsonSerializer.Deserialize<Invokes.TabActivity>(element.ToString(), options);
        }

        if (name.StartsWith("task/"))
        {
            return JsonSerializer.Deserialize<Invokes.TaskActivity>(element.ToString(), options);
        }

        return name switch
        {
            "actionableMessage/executeAction" => JsonSerializer.Deserialize<Invokes.ExecuteActionActivity>(element.ToString(), options),
            "fileConsent/invoke" => JsonSerializer.Deserialize<Invokes.FileConsentActivity>(element.ToString(), options),
            "handoff/action" => JsonSerializer.Deserialize<Invokes.HandoffActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<InvokeActivity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, InvokeActivity value, JsonSerializerOptions options)
    {
        if (value is Invokes.AdaptiveCardActivity adaptiveCard)
        {
            JsonSerializer.Serialize(writer, adaptiveCard, options);
            return;
        }

        if (value is Invokes.ConfigActivity config)
        {
            JsonSerializer.Serialize(writer, config, options);
            return;
        }

        if (value is Invokes.MessageExtensionActivity messageExtension)
        {
            JsonSerializer.Serialize(writer, messageExtension, options);
            return;
        }

        if (value is Invokes.MessageActivity message)
        {
            JsonSerializer.Serialize(writer, message, options);
            return;
        }

        if (value is Invokes.SignInActivity signIn)
        {
            JsonSerializer.Serialize(writer, signIn, options);
            return;
        }

        if (value is Invokes.TabActivity tab)
        {
            JsonSerializer.Serialize(writer, tab, options);
            return;
        }

        if (value is Invokes.TaskActivity task)
        {
            JsonSerializer.Serialize(writer, task, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}