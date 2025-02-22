using System.IdentityModel.Tokens.Jwt;

namespace Microsoft.Spark.Api.Auth;

public class JsonWebToken : IToken
{
    public string? AppId
    {
        get => (string?)_token.Payload.GetValueOrDefault("appid");
    }

    public string? AppDisplayName
    {
        get => (string?)_token.Payload.GetValueOrDefault("app_displayname");
    }

    public string? TenantId
    {
        get => (string?)_token.Payload.GetValueOrDefault("tid");
    }

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

    public CallerType From
    {
        get => AppId == null ? CallerType.Azure : CallerType.Bot;
    }

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