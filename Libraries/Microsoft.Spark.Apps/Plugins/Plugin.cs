namespace Microsoft.Spark.Apps.Plugins;

public interface IPlugin
{
    public event ErrorEventHandler ErrorEvent;
    public event StartEventHandler StartEvent;
    public event ActivityEventHandler ActivityEvent;

    public Task OnInit(IApp app);
    public Task OnStart(IApp app, Events.StartEventArgs args);
    public Task OnError(IApp app, Events.ErrorEventArgs args);
    public Task OnActivity(IApp app, IPlugin plugin, Events.ActivityEventArgs args);

    public delegate Task ErrorEventHandler(IPlugin sender, Events.ErrorEventArgs args);
    public delegate Task StartEventHandler(IPlugin sender, Events.StartEventArgs args);
    public delegate Task<object?> ActivityEventHandler(IPlugin sender, Events.ActivityEventArgs args);
}