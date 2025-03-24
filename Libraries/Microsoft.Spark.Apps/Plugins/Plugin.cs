namespace Microsoft.Spark.Apps.Plugins;

/// <summary>
/// a component for extending the base
/// `App` functionality
/// </summary>
public interface IPlugin
{
    public event ErrorEventHandler ErrorEvent;
    public event ActivityEventHandler ActivityEvent;

    public Task OnInit(IApp app);
    public Task OnStart(IApp app, Events.StartEventArgs args);
    public Task OnError(IApp app, Events.ErrorEventArgs args);
    public Task OnActivity(IApp app, ISender plugin, Events.ActivityEventArgs args);
    public Task OnActivitySent(IApp app, ISender plugin, Events.ActivitySentEventArgs args);
    public Task OnActivityResponse(IApp app, ISender plugin, Events.ActivityResponseEventArgs args);

    public delegate Task ErrorEventHandler(IPlugin sender, Events.ErrorEventArgs args);
    public delegate Task<Response?> ActivityEventHandler(ISender sender, Events.ActivityEventArgs args);
}