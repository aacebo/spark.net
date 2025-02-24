namespace Microsoft.Spark.Apps.Events;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class EventAttribute(string name, Type? type = null) : Attribute
{
    public readonly string Name = name;
    public readonly Type Type = type ?? typeof(EventArgs);
}