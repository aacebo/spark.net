namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Config Activity
/// </summary>
public abstract class ConfigInvokeActivity(Name.Configs name) : InvokeActivity(new(name.Value))
{
    public Configs.FetchActivity ToFetch()
    {
        return (Configs.FetchActivity)this;
    }

    public Configs.SubmitActivity ToSubmit()
    {
        return (Configs.SubmitActivity)this;
    }
}