using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Auth;

public delegate Task<ITokenResponse> TokenFactory(string? tenantId, params string[] scopes);

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

    public async Task<ITokenResponse> Resolve(IHttpClient _client, params string[] scopes)
    {
        return await Token(TenantId, scopes);
    }
}