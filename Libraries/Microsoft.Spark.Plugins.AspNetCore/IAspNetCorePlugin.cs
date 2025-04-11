using Microsoft.AspNetCore.Builder;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.Plugins.AspNetCore;

public interface IAspNetCorePlugin : IPlugin
{
    public IApplicationBuilder Configure(IApplicationBuilder builder);
}