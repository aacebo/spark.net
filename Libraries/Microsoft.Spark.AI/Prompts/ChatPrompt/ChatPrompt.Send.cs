using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    public async Task<IMessage> Send(IMessage message, TOptions? options = default)
    {
        var buffer = string.Empty;
        var prompt = Template != null ? await Template.Render() : null;
        var res = await Model.Send(message, new(Invoke, (_) => Task.Run(() => { }))
        {
            Functions = Functions.List,
            Messages = Messages,
            Prompt = prompt == null ? null : new DeveloperMessage(prompt),
            Options = options,
        });

        return res;
    }

    public Task<ModelMessage<string>> Send(string text, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        var message = UserMessage.Text(text);
        return Send((IMessage)message, options);
    }

    public Task<ModelMessage<string>> Send(IContent[] content, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        var message = UserMessage.Text(content);
        return Send((IMessage)message, options);
    }

    public Task<ModelMessage<string>> Send(UserMessage<string> message, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        return Send((IMessage)message, options);
    }

    public Task<ModelMessage<string>> Send(UserMessage<IEnumerable<IContent>> message, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        return Send((IMessage)message, options);
    }

    public async Task<ModelMessage<string>> Send(IMessage message, IChatPrompt<TOptions>.RequestOptions? options = null)
    {
        var messages = options?.Messages ?? Messages;
        var buffer = string.Empty;
        var onChunk = options?.OnChunk;
        var prompt = Template != null ? await Template.Render() : null;

        async Task OnChunk(string chunk)
        {
            if (chunk == string.Empty || onChunk == null) return;
            buffer += chunk;

            try
            {
                await onChunk(buffer);
                buffer = string.Empty;
            }
            catch { return; }
        }

        var res = await Model.Send(message, new(Invoke, OnChunk)
        {
            Functions = Functions.List,
            Messages = messages,
            Prompt = prompt == null ? null : new DeveloperMessage(prompt),
            Options = options == null ? default : options.Request,
        });

        return res;
    }
}