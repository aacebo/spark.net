using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public partial class Chat
{
    public Task<ModelMessage<string>> Send(UserMessage<string> message, ChatCompletionOptions? options)
    {
        throw new NotImplementedException();
    }

    public Task<IMessage> Send(IMessage message, ChatCompletionOptions? options)
    {
        throw new NotImplementedException();
    }
}