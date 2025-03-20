namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Tab Activity
/// </summary>
public abstract class TabInvokeActivity(Name.Tabs name) : InvokeActivity(new(name.Value))
{
    public Tabs.FetchActivity ToFetch()
    {
        return (Tabs.FetchActivity)this;
    }

    public Tabs.SubmitActivity ToSubmit()
    {
        return (Tabs.SubmitActivity)this;
    }
}