namespace Microsoft.Spark.Common.Http;

public interface IHttpClientFactory
{
    public IHttpClient CreateClient();
    public IHttpClient CreateClient(string name);
}