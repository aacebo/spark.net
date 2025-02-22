using Microsoft.Spark.Api;
using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps;

public interface IActivityContext<TActivity> where TActivity : IActivity
{
    public string Plugin { get; set; }
    public string AppId { get; set; }
    public ILogger Logger { get; set; }
    public TActivity Activity { get; set; }
    public ConversationReference Ref { get; set; }
}

public class ActivityContext<TActivity> : IActivityContext<TActivity> where TActivity : IActivity
{
    public required string Plugin { get; set; }
    public required string AppId { get; set; }
    public required ILogger Logger { get; set; }
    public required TActivity Activity { get; set; }
    public required ConversationReference Ref { get; set; }
}