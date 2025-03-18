using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Auth;

public delegate Task<ITokenResponse> TokenFactory(string? tenantId, params string[] scope);

public class TokenCredentials : IHttpCredentials
{
    public string ClientId { get; set; }
    public string? TenantId { get; set; }
    public TokenFactory Token { get; set; }

    public TokenCredentials(string clientId, TokenFactory token)
    {
        ClientId = clientId;
        Token = token;
    }

    public TokenCredentials(string clientId, string tenantId, TokenFactory token)
    {
        ClientId = clientId;
        TenantId = tenantId;
        Token = token;
    }

    public async Task<ITokenResponse> Resolve(IHttpClient _client)
    {
        return await Token(TenantId, "https://api.botframework.com/.default");
    }
}