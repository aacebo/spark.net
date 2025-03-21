using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;

namespace Microsoft.Spark.Apps.Plugins;

/// <summary>
/// a plugin that can send activities
/// </summary>
public interface ISender : IPlugin
{
    /// <summary>
    /// called by the `App`
    /// to send an activity
    /// </summary>
    /// <param name="activity">the activity to send</param>
    /// <param name="reference">the conversation reference</param>
    /// <returns>the sent activity</returns>
    public Task<Activity> Send(Activity activity, ConversationReference reference);

    /// <summary>
    /// called by the `App`
    /// to send an activity
    /// </summary>
    /// <typeparam name="TActivity">the activity type</typeparam>
    /// <param name="activity">the activity to send</param>
    /// <param name="reference">the conversation reference</param>
    /// <returns>the sent activity</returns>
    public Task<TActivity> Send<TActivity>(TActivity activity, ConversationReference reference) where TActivity : Activity;

    /// <summary>
    /// called by the `App`
    /// to create a new activity stream
    /// </summary>
    /// <param name="reference">the conversation reference</param>
    /// <returns>a new stream</returns>
    public IStreamer CreateStream(ConversationReference reference);
}