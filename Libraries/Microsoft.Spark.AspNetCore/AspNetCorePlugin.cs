
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Apps.Plugins;

namespace Microsoft.Spark.AspNetCore;

public class AspNetCorePlugin : IPlugin
{
    public string Name { get; } = "http";

    public Task OnActivity(IActivity activity)
    {
        throw new NotImplementedException();
    }

    public Task OnInit()
    {
        throw new NotImplementedException();
    }

    public Task OnStart()
    {
        throw new NotImplementedException();
    }
}