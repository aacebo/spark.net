namespace Microsoft.Spark.Apps.Plugins;

/// <summary>
/// a component for extending the base
/// `App` functionality
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// emitted when the plugin encounters an error
    /// </summary>
    public event ErrorEventHandler ErrorEvent;

    /// <summary>
    /// emitted when the plugin receives an activity
    /// </summary>
    public event ActivityEventHandler ActivityEvent;

    /// <summary>
    /// lifecycle method called by the `App` once during initialization
    /// </summary>
    public Task OnInit(IApp app);

    /// <summary>
    /// lifecycle method called by the `App` once during startup
    /// </summary>
    public Task OnStart(IApp app, Events.StartEventArgs args);

    /// <summary>
    /// called by the `App` when an error occurs
    /// </summary>
    public Task OnError(IApp app, Events.ErrorEventArgs args);

    /// <summary>
    /// called by the `App` when an activity is received
    /// </summary>
    public Task OnActivity(IApp app, ISender plugin, Events.ActivityEventArgs args);

    /// <summary>
    /// called by the `App` when an activity is sent
    /// </summary>
    public Task OnActivitySent(IApp app, ISender plugin, Events.ActivitySentEventArgs args);

    /// <summary>
    /// called by the `App` when an activity response is sent
    /// </summary>
    public Task OnActivityResponse(IApp app, ISender plugin, Events.ActivityResponseEventArgs args);

    public delegate Task ErrorEventHandler(IPlugin sender, Events.ErrorEventArgs args);
    public delegate Task<Response?> ActivityEventHandler(ISender sender, Events.ActivityEventArgs args);
}