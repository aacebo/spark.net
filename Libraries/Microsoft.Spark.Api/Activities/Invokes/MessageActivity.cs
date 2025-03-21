namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Message Activity
/// </summary>
public abstract class MessageActivity(Name.Messages name) : InvokeActivity(new(name.Value))
{
    public Messages.SubmitActionActivity ToSubmitAction() => (Messages.SubmitActionActivity)this;
}