namespace Microsoft.Spark.Apps;

public partial interface IContext
{
    [Flags]
    public enum Property
    {
        None = 0,
        AppId = 1,
        Activity = 2,
        Ref = 4,
        Context = AppId | Activity | Ref,
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class ActivityAttribute() : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public class LoggerAttribute() : Attribute
    {

    }
}