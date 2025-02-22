namespace Microsoft.Spark.Apps.Plugins;

public interface IPlugin
{
    public string Name { get; }

    public Task OnInit();
    public Task OnStart();
    public Task OnActivity(Api.Activities.IActivity activity);
}