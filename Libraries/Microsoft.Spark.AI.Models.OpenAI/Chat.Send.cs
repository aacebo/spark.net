using System.Text.Json;

using Microsoft.Spark.AI.Messages;

using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public partial class Chat
{
    public Task<IMessage> Send(IMessage message, ChatCompletionOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<ModelMessage<string>> Send(IMessage message, ChatModelOptions<ChatCompletionOptions> options)
    {
        var messages = options.Messages ?? [];

        if (message is ModelMessage<string> modelMessage && modelMessage.HasFunctionCalls)
        {
            foreach (var call in modelMessage.FunctionCalls ?? [])
            {
                var logger = Logger.Child($"Tools.{call.Name}");
                var startedAt = DateTime.Now;

                logger.Debug(call.Arguments);
                var content = string.Empty;

                try
                {
                    var args = call.Parse();
                    var res = await options.Invoke(call.Name, args);
                    content = res is string asString ? asString : JsonSerializer.Serialize(res);
                    logger.Debug(content);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    content = ex.Message;
                }

                messages.Add(new FunctionMessage()
                {
                    FunctionId = call.Id,
                    Content = content
                });
            }
        }

        messages = [.. messages];

        if (options.Prompt != null)
        {
            messages.Insert(0, options.Prompt);
        }

        var tools = options.Functions.Select(function => function.ToOpenAI()).ToArray();

        try
        {
            var requestOptions = options.Options ?? new ChatCompletionOptions();
            
            foreach (var tool in tools)
            {
                requestOptions.Tools.Add(tool);
            }

            var result = await ChatClient.CompleteChatAsync(
                messages.Select(message =>
                {
                    if (message is ModelMessage<string> modelMessage)
                    {
                        return modelMessage.ToOpenAI();
                    }

                    throw new Exception("invalid message type");
                }),
                requestOptions
            );
        }
        catch (Exception ex)
        {

        }
    }
}