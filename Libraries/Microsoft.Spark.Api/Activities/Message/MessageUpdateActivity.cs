using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType MessageUpdate = new("messageUpdate");
    public bool IsMessageUpdate => MessageUpdate.Equals(Value);
}

public class MessageUpdateActivity : MessageActivity
{
    public MessageUpdateActivity() : base()
    {
        Type = ActivityType.MessageUpdate;
    }

    public MessageUpdateActivity(string text) : base(text)
    {
        Type = ActivityType.MessageUpdate;
    }
}