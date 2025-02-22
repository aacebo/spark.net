using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ActivityAttribute(string? name = null, Type? type = null) : Attribute
{
    public readonly string Name = name ?? "activity";
    public readonly Type Type = type ?? typeof(IActivity);
}