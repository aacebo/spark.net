using System.Reflection;

using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public class AspNetCorePlugin : IPlugin
{
    public string Name { get; } = "Microsoft.Spark.AspNetCore";

    protected ILogger _logger;

    public AspNetCorePlugin(ILogger? logger = null)
    {
        logger ??= new ConsoleLogger(Assembly.GetEntryAssembly()?.GetName().Name ?? "@Spark");
        _logger = logger.Child(Name);
    }

    public Task OnInit(IApp app)
    {
        return Task.Run(() => _logger = app.Logger.Child(Name));
    }

    public Task OnStart()
    {
        return Task.Run(() => _logger.Info("OnStart"));
    }

    public Task OnActivity(IActivity activity)
    {
        return Task.Run(() => _logger.Info("OnActivity"));
    }
}