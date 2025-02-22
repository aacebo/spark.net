using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routing;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MessageAttribute() : ActivityAttribute("message", typeof(IMessageSendActivity))
{
}