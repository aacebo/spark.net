using Microsoft.Spark.AI.Messages;

namespace Microsoft.Spark.AI.Prompts;

/// <summary>
/// a prompt that can send/receive text
/// messages and expose chat model specific
/// features like streaming/functions
/// </summary>
public interface IChatPrompt : IPrompt
{
    /// <summary>
    /// invoke the prompt using string content
    /// </summary>
    /// <param name="text">the message text</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Invoke(string text);

    /// <summary>
    /// invoke the prompt using content blocks
    /// </summary>
    /// <param name="content">the message content</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Invoke(params IContent[] content);

    /// <summary>
    /// invoke the prompt
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Invoke(UserMessage<string> message);

    /// <summary>
    /// invoke the prompt
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <returns>the models response</returns>
    public Task<ModelMessage<string>> Invoke(UserMessage<IEnumerable<IContent>> message);
}