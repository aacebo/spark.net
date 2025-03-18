using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType MessageDelete = new("messageDelete");
    public bool IsMessageDelete => MessageDelete.Equals(Value);
}

public class MessageDeleteActivity() : Activity("messageDelete")
{
}