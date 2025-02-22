namespace Microsoft.Spark.Common.Http;

public interface IHttpCredentials
{
    public IHttpRequest OnRequest(IHttpRequest request);
}