using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Plugins;

/// <summary>
/// component that can send streamed chunks of an activity
/// </summary>
public interface IStreamer
{
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
    public Task Close();
}