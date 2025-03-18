namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageDeleteActivity : IMessageActivityBase
{
}

public class MessageDeleteActivity() : MessageActivityBase("messageDelete"), IMessageDeleteActivity
{
}