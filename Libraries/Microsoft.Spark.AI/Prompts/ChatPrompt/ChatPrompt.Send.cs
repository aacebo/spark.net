using Microsoft.Spark.AI.Messages;
using Microsoft.Spark.AI.Models;

namespace Microsoft.Spark.AI.Prompts;

public partial class ChatPrompt<TOptions>
{
    public async Task<IMessage> Send(IMessage message, TOptions? options = default)
    {
        var buffer = string.Empty;
        var prompt = Template != null ? await Template.Render() : null;
        var res = await Model.Send(message, new(Invoke)
        {
            Functions = Functions.List,
            Messages = Messages,
            Prompt = prompt == null ? null : new DeveloperMessage(prompt),
            Options = options,
        });

        return res;
    }

    public Task<ModelMessage<string>> Send(string text, IChatPrompt<TOptions>.RequestOptions? options = null, OnStreamChunk? onChunk = null)
    {
        var message = UserMessage.Text(text);
        return Send((IMessage)message, options, onChunk);
    }

    public Task<ModelMessage<string>> Send(IContent[] content, IChatPrompt<TOptions>.RequestOptions? options = null, OnStreamChunk? onChunk = null)
    {
        var message = UserMessage.Text(content);
        return Send((IMessage)message, options, onChunk);
    }

    public Task<ModelMessage<string>> Send(UserMessage<string> message, IChatPrompt<TOptions>.RequestOptions? options = null, OnStreamChunk? onChunk = null)
    {
        return Send((IMessage)message, options, onChunk);
    }

    public Task<ModelMessage<string>> Send(UserMessage<IEnumerable<IContent>> message, IChatPrompt<TOptions>.RequestOptions? options = null, OnStreamChunk? onChunk = null)
    {
        return Send((IMessage)message, options, onChunk);
    }

    public async Task<ModelMessage<string>> Send(IMessage message, IChatPrompt<TOptions>.RequestOptions? options = null, OnStreamChunk? onChunk = null)
    {
        var messages = options?.Messages ?? Messages;
        var buffer = string.Empty;
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

        ChatModelOptions<TOptions> requestOptions = new(Invoke)
        {
            Functions = Functions.List,
            Messages = messages,
            Prompt = prompt == null ? null : new DeveloperMessage(prompt),
            Options = options == null ? default : options.Request,
        };

        ModelMessage<string>? res;

        try
        {
            Logger.Debug(message);

            foreach (var plugin in Plugins)
            {
                message = await plugin.OnBeforeSend(this, message, requestOptions.Options);
            }

            if (onChunk == null)
            {
                res = await Model.Send(message, requestOptions);
            }
            else
            {
                res = await Model.Send(message, requestOptions, new Stream(OnChunk));
            }

            Logger.Debug(res);

            foreach (var plugin in Plugins)
            {
                res = (ModelMessage<string>)await plugin.OnAfterSend(this, res, requestOptions.Options);
            }

            return res;
        }
        catch (Exception ex)
        {
            ErrorEvent(Model, ex);
            throw new Exception("an error occured while attempting to send the message", ex);
        }
    }
}