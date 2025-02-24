namespace Microsoft.Spark.Apps;

public partial interface IApp
{
    public IPlugin? GetPlugin(Type type);
    public TPlugin? GetPlugin<TPlugin>() where TPlugin : IPlugin;
    public IApp AddPlugin(IPlugin plugin);
}

public partial class App
{
    protected IList<IPlugin> Plugins { get; set; }

    public IPlugin? GetPlugin(Type type)
    {
        return Plugins.SingleOrDefault(p => p.GetType() == type);
    }

    public TPlugin? GetPlugin<TPlugin>() where TPlugin : IPlugin
    {
        return (TPlugin?)Plugins.SingleOrDefault(p => p.GetType() == typeof(TPlugin));
    }

    public IApp AddPlugin(IPlugin plugin)
    {
        ErrorEvent += plugin.OnError;
        StartEvent += plugin.OnStart;
        ActivityReceivedEvent += async (app, args) =>
        {
            await plugin.OnActivity(app, args);
            return null;
        };

        plugin.ErrorEvent += (_, args) => ErrorEvent(this, args);
        plugin.StartEvent += (_, args) => StartEvent(this, args);
        plugin.ActivityReceivedEvent += (_, args) => ActivityReceivedEvent(this, args);

        plugin.OnInit(this);
        Plugins.Add(plugin);
        return this;
    }
}