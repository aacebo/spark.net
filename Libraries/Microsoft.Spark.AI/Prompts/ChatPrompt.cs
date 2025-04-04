using Microsoft.Spark.AI.Messages;
using Microsoft.Spark.AI.Models;

namespace Microsoft.Spark.AI.Prompts;

/// <summary>
/// a prompt that can send/receive text
/// messages and expose chat model specific
/// features like streaming/functions
/// </summary>
public interface IChatPrompt<TOptions> : IPrompt<TOptions>
{
    /// <summary>
    /// send a message via the prompt using string content
    /// </summary>
    /// <param name="text">the message text</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(string text, RequestOptions? options = null);

    /// <summary>
    /// send a message via the prompt using content blocks
    /// </summary>
    /// <param name="content">the message content</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(IContent[] content, RequestOptions? options = null);

    /// <summary>
    /// send a message via the prompt
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(UserMessage<string> message, RequestOptions? options = null);

    /// <summary>
    /// send a message via the prompt
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(UserMessage<IEnumerable<IContent>> message, RequestOptions? options = null);

    /// <summary>
    /// options to send when invoking a prompt
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// the conversation history
        /// </summary>
        public IList<IMessage>? Messages { get; set; }

        /// <summary>
        /// the model request options
        /// </summary>
        public TOptions? Request { get; set; }

        /// <summary>
        /// the handler called when a stream chunk is
        /// emitted by the model.
        /// If not null, streaming is enabled.
        /// </summary>
        public Func<string, Task>? OnChunk { get; set; }
    }
}

/// <summary>
/// a prompt that can send/receive text
/// messages and expose chat model specific
/// features like streaming/functions
/// </summary>
public partial class ChatPrompt<TOptions> : IChatPrompt<TOptions>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IList<IMessage> Messages { get; private set; }
    public FunctionCollection Functions { get; private set; }

    protected IChatModel<TOptions> Model { get; }
    protected ITemplate? Template { get; }

    public ChatPrompt(IChatModel<TOptions> model, ChatPromptOptions? options = null)
    {
        options ??= new();
        Name = options.Name ?? "chat";
        Description = options.Description ?? "an agent you can chat with";
        Model = model;
        Template = options.Instructions;
        Messages = options.Messages ?? [];
        Functions = new();
    }
}