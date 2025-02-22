namespace Microsoft.Spark.Common.Http;

using IHttpHeaders = IDictionary<string, IList<string>>;

public interface IHttpRequestOptions
{
    public IHttpHeaders Headers { get; set; }
}

public class HttpRequestOptions : IHttpRequestOptions
{
    public IHttpHeaders Headers { get; set; } = new Dictionary<string, IList<string>>();
}