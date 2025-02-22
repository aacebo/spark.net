namespace Microsoft.Spark.Apps;

public interface IAppOptions
{
    public Common.Logging.ILogger? Logger { get; set; }
    public Common.Http.IHttpClient? Http { get; set; }
    public Common.Http.IHttpClientFactory? HttpFactory { get; set; }
    public Common.Http.IHttpCredentials? Credentials { get; set; }
    public IList<IPlugin> Plugins { get; set; }
}

public class AppOptions : IAppOptions
{
    public Common.Logging.ILogger? Logger { get; set; }
    public Common.Http.IHttpClient? Http { get; set; }
    public Common.Http.IHttpClientFactory? HttpFactory { get; set; }
    public Common.Http.IHttpCredentials? Credentials { get; set; }
    public IList<IPlugin> Plugins { get; set; } = [];
}