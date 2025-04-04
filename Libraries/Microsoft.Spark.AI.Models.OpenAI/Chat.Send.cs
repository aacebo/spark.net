using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public partial class Chat
{
    public Task<IMessage> Send(IMessage message, ChatCompletionOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<ModelMessage<string>> Send(IMessage message, ChatModelOptions<ChatCompletionOptions> options)
    {
        throw new NotImplementedException();
    }
}