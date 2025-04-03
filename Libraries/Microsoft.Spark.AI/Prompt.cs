namespace Microsoft.Spark.AI;

/// <summary>
/// prompts act as an orchestrator for models.
/// they supply a common interface with useful feature
/// abstractions that differ with the prompts supported
/// media types
/// </summary>
public interface IPrompt
{
    /// <summary>
    /// the prompt name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// the prompt description
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// invoke the prompt
    /// </summary>
    /// <param name="message">the message to send to the model</param>
    /// <returns>the models response</returns>
    public Task<IMessage> Invoke(IMessage message);
}