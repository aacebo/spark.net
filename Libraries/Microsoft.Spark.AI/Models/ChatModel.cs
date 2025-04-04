using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Models;

/// <summary>
/// a model that can reason over and
/// respond with text
/// </summary>
public interface IChatModel<TOptions> : IModel<TOptions>
{
    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the chat request</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(Request request);

    /// <summary>
    /// options to send with the message
    /// </summary>
    public class Request
    {
        /// <summary>
        /// the message to send to the model
        /// </summary>
        public IMessage Input { get; set; }

        /// <summary>
        /// the initial prompt message that defines
        /// model behavior
        /// </summary>
        public DeveloperMessage? Prompt { get; set; }

        /// <summary>
        /// the conversation history
        /// </summary>
        public IList<IMessage>? Messages { get; set; }

        /// <summary>
        /// the registered functions that can be
        /// called
        /// </summary>
        public required IList<IFunction> Functions { get; set; }

        /// <summary>
        /// the request options defined by the model
        /// </summary>
        public TOptions? Options { get; set; }

        /// <summary>
        /// the handler used to invoke functions
        /// </summary>
        internal Func<string, object?, Task<object?>> InvokeHandler { get; set; }

        /// <summary>
        /// the handler used to emit string chunks
        /// </summary>
        internal Action<string> ChunkHandler { get; set; }

        /// <summary>
        /// invoke a function
        /// </summary>
        /// <param name="name">the function name</param>
        /// <param name="args">the function args</param>
        /// <returns>the function response</returns>
        public Task<object?> Invoke(string name, object? args)
        {
            return InvokeHandler(name, args);
        }

        public void Emit(string chunk)
        {
            ChunkHandler(chunk);
        }
    }
}