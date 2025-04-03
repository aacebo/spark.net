using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Models;

/// <summary>
/// a model that can reason over and
/// respond with text
/// </summary>
public interface IChatModel : IModel
{
    /// <summary>
    /// send a message to the model
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Send(UserMessage<string> message);
}