namespace Microsoft.Spark.Apps;

public interface IPlugin
{
    public string Name { get; }

    public Task OnInit();
    public Task OnStart();
    public Task OnActivity(Api.Activities.IActivity activity);
}