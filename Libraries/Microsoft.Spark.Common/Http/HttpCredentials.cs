namespace Microsoft.Spark.Common.Http;

public interface IHttpCredentials
{
    public Task<ITokenResponse> Resolve(IHttpClient client, string[] scopes, CancellationToken cancellationToken = default);
}