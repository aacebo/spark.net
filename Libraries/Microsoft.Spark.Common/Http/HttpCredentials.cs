namespace Microsoft.Spark.Common.Http;

public interface IHttpCredentials
{
    public Task<ITokenResponse> Resolve(IHttpClient client, params string[] scopes);
}