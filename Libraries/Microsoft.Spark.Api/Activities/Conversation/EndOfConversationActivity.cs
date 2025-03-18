namespace Microsoft.Spark.Api.Activities.Conversation;

public interface IEndOfConversationActivity : IConversationActivityBase
{

}

public class EndOfConversationActivity : ConversationActivityBase, IEndOfConversationActivity
{

}