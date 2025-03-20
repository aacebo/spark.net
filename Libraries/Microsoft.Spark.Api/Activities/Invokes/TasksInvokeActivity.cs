namespace Microsoft.Spark.Api.Activities.Invokes;

/// <summary>
/// Any Task Activity
/// </summary>
public abstract class TaskInvokeActivity(Name.Tasks name) : InvokeActivity(new(name.Value))
{
    public Tasks.FetchActivity ToFetch()
    {
        return (Tasks.FetchActivity)this;
    }

    public Tasks.SubmitActivity ToSubmit()
    {
        return (Tasks.SubmitActivity)this;
    }
}