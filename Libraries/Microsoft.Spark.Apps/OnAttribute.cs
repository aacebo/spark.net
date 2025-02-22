using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OnAttribute(string name, Type? type = null) : Attribute
{
    public readonly string Name = name;
    public readonly Type Type = type ?? typeof(IActivity);
}