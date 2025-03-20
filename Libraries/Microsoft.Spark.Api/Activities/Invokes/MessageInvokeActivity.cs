namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Message Activity
/// </summary>
public abstract class MessageInvokeActivity(Name.Messages name) : InvokeActivity(new(name.Value))
{
    public Messages.SubmitActionActivity ToSubmitAction()
    {
        return (Messages.SubmitActionActivity)this;
    }
}