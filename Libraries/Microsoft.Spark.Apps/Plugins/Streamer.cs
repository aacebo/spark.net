using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Plugins;

/// <summary>
/// component that can send streamed chunks of an activity
/// </summary>
public interface IStreamer
{
    /// <summary>
    /// whether the final stream
    /// message has been sent
    /// </summary>
    public bool Closed { get; }

    /// <summary>
    /// event emitted on each chunk send
    /// </summary>
    public event OnChunkHandler OnChunk;

    /// <summary>
    /// emit an activity
    /// </summary>
    /// <param name="activity">the activity</param>
    public void Emit(MessageActivity activity);

    /// <summary>
    /// emit an activity
    /// </summary>
    /// <param name="activity">the activity</param>
    public void Emit(TypingActivity activity);

    /// <summary>
    /// emit text chunk
    /// </summary>
    /// <param name="text">the text</param>
    public void Emit(string text);

    /// <summary>
    /// close the stream
    /// </summary>
    public Task<MessageActivity> Close();

    /// <summary>
    /// handler called on each chunk send
    /// </summary>
    public delegate void OnChunkHandler(IActivity activity);
}