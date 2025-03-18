using System.Text.Json.Serialization;

using Microsoft.Spark.Common.Http;

namespace Microsoft.Spark.Api.Auth;

public class TokenResponse : ITokenResponse
{
    [JsonPropertyName("token_type")]
    [JsonPropertyOrder(0)]
    public required string TokenType { get; init; }

    [JsonPropertyName("expires_in")]
    [JsonPropertyOrder(1)]
    public int? ExpiresIn { get; }

    [JsonPropertyName("access_token")]
    [JsonPropertyOrder(2)]
    public required string AccessToken { get; init; }
}