namespace Microsoft.Spark.Apps;

public partial interface IContext
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ActivityAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class LoggerAttribute() : Attribute
    {

    }
}