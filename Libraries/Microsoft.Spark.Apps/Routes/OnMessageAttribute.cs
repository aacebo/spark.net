using Microsoft.Spark.Api.Activities.Message;

namespace Microsoft.Spark.Apps.Routes;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OnMessageAttribute() : OnAttribute("message", typeof(IMessageSendActivity))
{
}