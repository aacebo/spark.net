namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Tab Activity
/// </summary>
public abstract class TabActivity(Name.Tabs name) : InvokeActivity(new(name.Value))
{
    public Tabs.FetchActivity ToFetch() => (Tabs.FetchActivity)this;
    public Tabs.SubmitActivity ToSubmit() => (Tabs.SubmitActivity)this;
}