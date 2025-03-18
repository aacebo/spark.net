namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageDeleteActivity : IMessageActivityBase
{
}

public class MessageDeleteActivity : MessageActivityBase, IMessageDeleteActivity
{
    public MessageDeleteActivity() : base()
    {
        Type = "messageDelete";
    }
}