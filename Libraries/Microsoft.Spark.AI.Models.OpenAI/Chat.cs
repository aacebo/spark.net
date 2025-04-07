using System.ClientModel;

using Microsoft.Spark.AI.Prompts;
using Microsoft.Spark.Common.Logging;

using OpenAI;
using OpenAI.Chat;

namespace Microsoft.Spark.AI.Models.OpenAI;

public class OpenAIChatPrompt : ChatPrompt<ChatCompletionOptions>
{
    public OpenAIChatPrompt(Chat model, ChatPromptOptions? options = null) : base(model, options)
    {

    }

    public static IChatPrompt<ChatCompletionOptions> From<T>(Chat model, T value) where T : class
    {
        return ChatPrompt<ChatCompletionOptions>.From(model, value);
    }
}


public partial class Chat : IChatModel<ChatCompletionOptions>
{
    public string Name => throw new NotImplementedException();

    /// <summary>
    /// the OpenAI client used to
    /// make requests
    /// </summary>
    public OpenAIClient Client { get; set; }

    /// <summary>
    /// the OpenAI chat client used to
    /// make requests
    /// </summary>
    public ChatClient ChatClient { get; set; }

    /// <summary>
    /// the model name
    /// </summary>
    protected string Model { get; set; }

    /// <summary>
    /// the logger instance
    /// </summary>
    protected ILogger Logger { get; set; }

    public Chat(string model, string apiKey, Options? options = null)
    {
        Model = model;
        Client = new(new ApiKeyCredential(apiKey), options ?? new());
        ChatClient = Client.GetChatClient(model);
        Logger = (options?.Logger ?? new ConsoleLogger()).Child(model);
    }

    public Chat(string model, ApiKeyCredential apiKey, Options? options = null)
    {
        Model = model;
        Client = new(apiKey, options ?? new());
        ChatClient = Client.GetChatClient(model);
        Logger = (options?.Logger ?? new ConsoleLogger()).Child(model);
    }
}