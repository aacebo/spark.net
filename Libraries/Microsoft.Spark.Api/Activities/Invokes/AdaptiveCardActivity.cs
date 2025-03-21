namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any AdaptiveCard Activity
/// </summary>
public abstract class AdaptiveCardActivity(Name.AdaptiveCards name) : InvokeActivity(new(name.Value))
{
    public AdaptiveCards.ActionActivity ToAction()
    {
        return (AdaptiveCards.ActionActivity)this;
    }
}