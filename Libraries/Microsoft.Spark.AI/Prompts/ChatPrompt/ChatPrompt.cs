using System.Reflection;

using Json.Schema;
using Json.Schema.Generation;

using Microsoft.Spark.AI.Annotations;
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

    /// <summary>
    /// create a ChatPrompt from any class
    /// utilizing the ChatPromptAttribute
    /// </summary>
    /// <param name="model">the model to use</param>
    /// <param name="value">the class instance to use</param>
    /// <returns>a ChatPrompt</returns>
    /// <exception cref="Exception"></exception>
    public static ChatPrompt<TOptions> From<T>(IChatModel<TOptions> model, T value) where T : class
    {
        var type = value.GetType();
        var promptAttribute = type.GetCustomAttribute<PromptAttribute>();
        var nameAttribute = type.GetCustomAttribute<Prompt.NameAttribute>();
        var descriptionAttribute = type.GetCustomAttribute<Prompt.DescriptionAttribute>();
        var instructionsAttribute = type.GetCustomAttribute<Prompt.InstructionsAttribute>();

        if (promptAttribute == null)
        {
            throw new Exception("only types utilizing the ChatPromptAttribute can be turned into a ChatPrompt");
        }

        var name = promptAttribute.Name ?? nameAttribute?.Name ?? type.Name;
        var description = promptAttribute.Description ?? descriptionAttribute?.Description;
        var instructions = promptAttribute.Instructions ?? instructionsAttribute?.Instructions;
        var options = new ChatPromptOptions().WithName(name);

        if (description != null)
        {
            options = options.WithDescription(description);
        }

        if (instructions != null)
        {
            options = options.WithInstructions(instructions);
        }

        var prompt = new ChatPrompt<TOptions>(model, options);

        foreach (var method in type.GetMethods())
        {
            var functionAttribute = method.GetCustomAttribute<FunctionAttribute>();
            var functionDescriptionAttribute = method.GetCustomAttribute<Annotations.Function.DescriptionAttribute>();

            if (functionAttribute == null) continue;

            var parameters = method.GetParameters();

            if (parameters.Length > 1)
            {
                throw new Exception("invalid ChatPrompt Function signature, at most 1 parameter is allowed");
            }

            var function = new Function(
                functionAttribute.Name ?? method.Name,
                functionAttribute.Description ?? functionDescriptionAttribute?.Description,
                async (args) =>
                {
                    var res = method.Invoke(value, [args]);

                    if (res is Task<object?> task)
                        return await task;

                    return res;
                }
            );

            var param = parameters.FirstOrDefault();
            function.Parameters = param == null ? null : new JsonSchemaBuilder().FromType(param.ParameterType).Build();
            prompt.Function(function);
        }

        return prompt;
    }
}