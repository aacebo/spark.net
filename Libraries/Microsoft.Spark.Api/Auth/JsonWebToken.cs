using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Auth;

public class JsonWebToken : IToken
{
    [JsonPropertyName("appid")]
    public string? AppId
    {
        get => (string?)_token.Payload.GetValueOrDefault("appid");
    }

    [JsonPropertyName("app_displayname")]
    public string? AppDisplayName
    {
        get => (string?)_token.Payload.GetValueOrDefault("app_displayname");
    }

    [JsonPropertyName("tid")]
    public string? TenantId
    {
        get => (string?)_token.Payload.GetValueOrDefault("tid");
    }

    [JsonPropertyName("serviceurl")]
    public string ServiceUrl
    {
        get
        {
            var value = ((string?)_token.Payload.GetValueOrDefault("serviceurl")) ?? "https://smba.trafficmanager.net/teams";

            if (value.EndsWith('/'))
            {
                value = value[..^1];
            }

            return value;
        }
    }

    [JsonPropertyName("from")]
    public CallerType From
    {
        get => AppId == null ? CallerType.Azure : CallerType.Bot;
    }

    [JsonPropertyName("fromId")]
    public string FromId
    {
        get => From.IsBot ? $"urn:botframework:aadappid:{AppId}" : "urn:botframework:azure";
    }

    private readonly JwtSecurityToken _token;
    private readonly string _tokenAsString;

    public JsonWebToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        _token = handler.ReadJwtToken(token);
        _tokenAsString = token;
    }

    public override string ToString() => _tokenAsString;
}