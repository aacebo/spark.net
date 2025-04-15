using System.Reflection;

using Microsoft.Teams.Apps.Plugins;
using Microsoft.Teams.Common.Logging;

namespace Microsoft.Teams.Apps;

public partial interface IApp
{
    public IPlugin? GetPlugin(string name);
    public IPlugin? GetPlugin(Type type);
    public TPlugin? GetPlugin<TPlugin>() where TPlugin : IPlugin;
    public IApp AddPlugin(IPlugin plugin);
}

public partial class App
{
    protected IList<IPlugin> Plugins { get; set; }

    public IPlugin? GetPlugin(string name)
    {
        return Plugins.SingleOrDefault(p => GetPluginAttribute(p).Name == name);
    }

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
        var attr = GetPluginAttribute(plugin);

        // broadcast plugin events
        plugin.ErrorEvent += (sender, exception) => ErrorEvent(this, sender, exception, null);
        plugin.ActivityEvent += OnActivityEvent;
        Plugins.Add(plugin);
        Container.Register(attr.Name, new ValueProvider(plugin));
        Container.Register(plugin.GetType().Name, new ValueProvider(plugin));
        Logger.Debug($"plugin {attr.Name} added");
        return this;
    }

    protected static PluginAttribute GetPluginAttribute(IPlugin plugin)
    {
        var attribute = (PluginAttribute?)Attribute.GetCustomAttribute(plugin.GetType(), typeof(PluginAttribute));

        if (attribute == null)
        {
            throw new InvalidOperationException($"type '{plugin.GetType().Name}' is not a valid plugin");
        }

        return attribute;
    }

    protected void Inject(IPlugin plugin)
    {
        var metadata = GetPluginAttribute(plugin);
        var properties = plugin
            .GetType()
            .GetProperties()
            .Where(property => property.IsDefined(typeof(DependencyAttribute), true));

        foreach (var property in properties)
        {
            var attribute = property.GetCustomAttribute<DependencyAttribute>();

            if (attribute == null) continue;

            var dependency = Container.Resolve<object>(attribute.Name ?? property.PropertyType.Name);

            if (dependency == null)
            {
                dependency = Container.Resolve<object>(property.Name);
            }

            if (dependency == null)
            {
                if (attribute.Optional) continue;
                throw new InvalidOperationException($"dependency '{property.PropertyType.Name}' of property '{property.Name}' not found, but plugin '{metadata.Name}' depends on it");
            }

            if (dependency is ILogger logger)
            {
                dependency = logger.Child(metadata.Name);
            }

            property.SetValue(plugin, dependency);
        }
    }
}