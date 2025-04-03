using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Models;

/// <summary>
/// a model that can reason over audio
/// </summary>
public interface IAudioModel<TOptions> : IModel<TOptions>
{
    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<Stream>> Send(UserMessage<string> message, TOptions? options);

    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(UserMessage<Stream> message, TOptions? options);
}