namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public IApp OnError(ErrorEventHandler handler);
    public IApp OnStart(StartEventHandler handler);
    public IApp OnActivityReceived(ActivityReceivedEventHandler handler);

    public delegate Task ErrorEventHandler(IApp app, Events.ErrorEventArgs args);
    public delegate Task StartEventHandler(IApp app, Events.StartEventArgs args);
    public delegate Task<object?> ActivityReceivedEventHandler(IApp app, Events.ActivityReceivedEventArgs args);
}

public partial class App
{
    protected event IApp.ErrorEventHandler ErrorEvent = (app, args) => Task.Run(() => { });
    protected event IApp.StartEventHandler StartEvent = (app, args) => Task.Run(() => { });
    protected event IApp.ActivityReceivedEventHandler ActivityReceivedEvent = (app, args) => Task.Run(() => (object?)null);

    public IApp OnError(IApp.ErrorEventHandler handler)
    {
        ErrorEvent += handler;
        return this;
    }

    public IApp OnStart(IApp.StartEventHandler handler)
    {
        StartEvent += handler;
        return this;
    }

    public IApp OnActivityReceived(IApp.ActivityReceivedEventHandler handler)
    {
        ActivityReceivedEvent += handler;
        return this;
    }
}