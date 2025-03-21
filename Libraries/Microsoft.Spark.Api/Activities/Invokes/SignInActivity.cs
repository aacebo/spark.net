using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any SignIn Activity
/// </summary>
[JsonConverter(typeof(SignInActivityJsonConverter))]
public abstract class SignInActivity(Name.SignIn name) : InvokeActivity(new(name.Value))
{
    public SignIn.TokenExchangeActivity ToTokenExchange() => (SignIn.TokenExchangeActivity)this;
    public SignIn.VerifyStateActivity ToVerifyState() => (SignIn.VerifyStateActivity)this;
}

public class SignInActivityJsonConverter : JsonConverter<SignInActivity>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return base.CanConvert(typeToConvert);
    }

    public override SignInActivity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            "signin/tokenExchange" => JsonSerializer.Deserialize<SignIn.TokenExchangeActivity>(element.ToString(), options),
            "signin/verifyState" => JsonSerializer.Deserialize<SignIn.VerifyStateActivity>(element.ToString(), options),
            _ => JsonSerializer.Deserialize<SignInActivity>(element.ToString(), options)
        };
    }

    public override void Write(Utf8JsonWriter writer, SignInActivity value, JsonSerializerOptions options)
    {
        if (value is SignIn.TokenExchangeActivity tokenExchange)
        {
            JsonSerializer.Serialize(writer, tokenExchange, options);
            return;
        }

        if (value is SignIn.VerifyStateActivity verifyState)
        {
            JsonSerializer.Serialize(writer, verifyState, options);
            return;
        }

        JsonSerializer.Serialize(writer, value, options);
    }
}