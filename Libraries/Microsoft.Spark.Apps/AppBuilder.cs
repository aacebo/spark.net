using System.Reflection;

namespace Microsoft.Spark.Apps;

public interface IAppBuilder
{
    public IAppBuilder AddLogger(Common.Logging.ILogger logger);
    public IAppBuilder AddLogger(string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info);

    public IAppBuilder AddHttp(Common.Http.IHttpClient client);
    public IAppBuilder AddHttp(Common.Http.IHttpClientFactory factory);
    public IAppBuilder AddHttp(Func<Common.Http.IHttpClient> @delegate);
    public IAppBuilder AddHttp(Func<Task<Common.Http.IHttpClient>> @delegate);

    public IAppBuilder AddCredentials(Common.Http.IHttpCredentials credentials);
    public IAppBuilder AddCredentials(Func<Common.Http.IHttpCredentials> @delegate);
    public IAppBuilder AddCredentials(Func<Task<Common.Http.IHttpCredentials>> @delegate);

    public IAppBuilder AddPlugin(IPlugin plugin);
    public IAppBuilder AddPlugin(Func<IPlugin> @delegate);
    public IAppBuilder AddPlugin(Func<Task<IPlugin>> @delegate);

    public IApp Build();
}

public partial class AppBuilder : IAppBuilder
{
    protected IAppOptions _options;

    public AppBuilder(IAppOptions? options = null)
    {
        _options = options ?? new AppOptions();
    }

    public IAppBuilder AddLogger(Common.Logging.ILogger logger)
    {
        _options.Logger = logger;
        return this;
    }

    public IAppBuilder AddLogger(string? name = null, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        name ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark";
        _options.Logger = new Common.Logging.ConsoleLogger(name, level);
        return this;
    }

    public IAppBuilder AddHttp(Common.Http.IHttpClient client)
    {
        _options.Http = client;
        return this;
    }

    public IAppBuilder AddHttp(Common.Http.IHttpClientFactory factory)
    {
        _options.HttpFactory = factory;
        return this;
    }

    public IAppBuilder AddHttp(Func<Common.Http.IHttpClient> @delegate)
    {
        _options.Http = @delegate();
        return this;
    }

    public IAppBuilder AddHttp(Func<Task<Common.Http.IHttpClient>> @delegate)
    {
        _options.Http = @delegate().GetAwaiter().GetResult();
        return this;
    }

    public IAppBuilder AddCredentials(Common.Http.IHttpCredentials credentials)
    {
        _options.Credentials = credentials;
        return this;
    }

    public IAppBuilder AddCredentials(Func<Common.Http.IHttpCredentials> @delegate)
    {
        _options.Credentials = @delegate();
        return this;
    }

    public IAppBuilder AddCredentials(Func<Task<Common.Http.IHttpCredentials>> @delegate)
    {
        _options.Credentials = @delegate().GetAwaiter().GetResult();
        return this;
    }

    public IAppBuilder AddPlugin(IPlugin plugin)
    {
        _options.Plugins.Add(plugin);
        return this;
    }

    public IAppBuilder AddPlugin(Func<IPlugin> @delegate)
    {
        _options.Plugins.Add(@delegate());
        return this;
    }

    public IAppBuilder AddPlugin(Func<Task<IPlugin>> @delegate)
    {
        _options.Plugins.Add(@delegate().GetAwaiter().GetResult());
        return this;
    }

    public IApp Build()
    {
        return new App(_options);
    }
}