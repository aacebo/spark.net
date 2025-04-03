using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Models;

/// <summary>
/// a model that can reason over images
/// </summary>
public interface IImageModel : IModel
{
    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<Stream>> Send(UserMessage<string> message);

    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(UserMessage<Stream> message);
}