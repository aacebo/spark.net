namespace Microsoft.Spark.Api.Activities.Message;

public interface IMessageDeleteActivity : IActivity
{
}

public class MessageDeleteActivity() : Activity("messageDelete"), IMessageDeleteActivity
{
}