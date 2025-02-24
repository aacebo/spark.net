namespace Microsoft.Spark.Apps;

public interface IPlugin
{
    public string Name { get; }

    public event ErrorEventHandler ErrorEvent;
    public event StartEventHandler StartEvent;
    public event ActivityReceivedEventHandler ActivityReceivedEvent;

    public Task OnInit(IApp app);
    public Task OnStart(IApp app, Events.StartEventArgs args);
    public Task OnActivity(IApp app, Events.ActivityReceivedEventArgs args);
    public Task OnError(IApp app, Events.ErrorEventArgs args);

    public delegate Task ErrorEventHandler(IPlugin sender, Events.ErrorEventArgs args);
    public delegate Task StartEventHandler(IPlugin sender, Events.StartEventArgs args);
    public delegate Task<object?> ActivityReceivedEventHandler(IPlugin sender, Events.ActivityReceivedEventArgs args);
}