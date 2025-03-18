namespace Microsoft.Spark.Apps.Plugins;

public class PluginOptions
{
    public string? Name { get; set; }
    public string? Version { get; set; }
    public string? Description { get; set; }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class PluginAttribute() : Attribute
{
    public required string Name { get; set; }
    public string Version { get; set; } = "0.0.0";
    public string? Description { get; set; }
}