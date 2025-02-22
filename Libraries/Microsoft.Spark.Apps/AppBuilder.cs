using System.Reflection;

using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.Apps;

public interface IAppBuilder
{
    public IApp Build();
}

public partial class AppBuilder : IAppBuilder
{
    protected IAppOptions _options;

    public AppBuilder(IAppOptions? options = null)
    {
        _options = options ?? new AppOptions();
    }

    public AppBuilder AddLogger(Common.Logging.ILogger logger)
    {
        _options.Logger = logger;
        return this;
    }

    public AppBuilder AddLogger(string? name, Common.Logging.LogLevel level = Common.Logging.LogLevel.Info)
    {
        name ??= Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark";
        _options.Logger = new Common.Logging.ConsoleLogger(name, level);
        return this;
    }

    public AppBuilder AddHttp(Common.Http.IHttpClient client)
    {
        _options.Http = client;
        return this;
    }

    public AppBuilder AddHttp(Common.Http.IHttpClientFactory factory)
    {
        _options.HttpFactory = factory;
        return this;
    }

    public AppBuilder AddHttp(Func<Common.Http.IHttpClient> @delegate)
    {
        _options.Http = @delegate();
        return this;
    }

    public AppBuilder AddHttp(Func<Task<Common.Http.IHttpClient>> @delegate)
    {
        _options.Http = @delegate().GetAwaiter().GetResult();
        return this;
    }

    public AppBuilder AddCredentials(Common.Http.IHttpCredentials credentials)
    {
        _options.Credentials = credentials;
        return this;
    }

    public AppBuilder AddCredentials(Func<Common.Http.IHttpCredentials> @delegate)
    {
        _options.Credentials = @delegate();
        return this;
    }

    public AppBuilder AddCredentials(Func<Task<Common.Http.IHttpCredentials>> @delegate)
    {
        _options.Credentials = @delegate().GetAwaiter().GetResult();
        return this;
    }

    public AppBuilder AddPlugin(IPlugin plugin)
    {
        _options.Plugins.Add(plugin);
        return this;
    }

    public AppBuilder AddPlugin(Func<IPlugin> @delegate)
    {
        _options.Plugins.Add(@delegate());
        return this;
    }

    public AppBuilder AddPlugin(Func<Task<IPlugin>> @delegate)
    {
        _options.Plugins.Add(@delegate().GetAwaiter().GetResult());
        return this;
    }

    public IApp Build()
    {
        return new App(_options);
    }
}