namespace Microsoft.Spark.Apps.Routes;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OnActivityAttribute() : OnAttribute("activity")
{
}