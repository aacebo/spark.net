using Microsoft.Spark.Api.Activities;
using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.Apps.Routing;

public interface IContext<TActivity> where TActivity : Activity
{
    public string AppId { get; set; }
    public ILogger Logger { get; set; }
    public TActivity Activity { get; set; }
    // public ConversationReference Ref { get; set; }
    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : TActivity;
}

public class Context<TActivity> : IContext<TActivity> where TActivity : Activity
{
    public required string AppId { get; set; }
    public required ILogger Logger { get; set; }
    public required TActivity Activity { get; set; }
    // public required ConversationReference Ref { get; set; }

    public IContext<TToActivity> ToActivityType<TToActivity>() where TToActivity : TActivity
    {
        return new Context<TToActivity>()
        {
            AppId = AppId,
            Logger = Logger,
            Activity = (TToActivity)Activity
        };
    }
}