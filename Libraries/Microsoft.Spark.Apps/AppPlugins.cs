using Microsoft.Spark.Apps.Plugins;

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
        // subscribe plugin to app events
        ErrorEvent += plugin.OnError;
        StartEvent += plugin.OnStart;
        ActivityEvent += async (app, sender, args) =>
        {
            await plugin.OnActivity(app, sender, args);
            return null;
        };

        // broadcast plugin events
        plugin.ErrorEvent += (_, args) => ErrorEvent(this, args);
        plugin.ActivityEvent += async (plugin, args) =>
        {
            await ActivityEvent(this, plugin, args);
            return null;
        };

        Plugins.Add(plugin);
        return this;
    }
}