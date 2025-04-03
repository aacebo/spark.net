using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    public Task<ModelMessage<string>> Send(string text, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<ModelMessage<string>> Send(IContent[] content, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<ModelMessage<string>> Send(UserMessage<string> message, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<ModelMessage<string>> Send(UserMessage<IEnumerable<IContent>> message, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<IMessage> Send(IMessage message, TOptions? options = default)
    {
        throw new NotImplementedException();
    }
}