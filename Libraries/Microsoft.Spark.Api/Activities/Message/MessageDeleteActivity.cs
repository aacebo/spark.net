namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageDeleteActivity : IMessageActivity
{
}

public class MessageDeleteActivity : MessageActivity, IMessageDeleteActivity
{
    public MessageDeleteActivity() : base()
    {
        Type = "messageDelete";
    }
}