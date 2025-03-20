using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public static readonly Name ConfigFetch = new("config/fetch");
    public bool IsConfigFetch => ConfigFetch.Equals(Value);
}

public class ConfigFetchActivity : InvokeActivity
{
    public ConfigFetchActivity(object? value) : base(Name.ConfigFetch)
    {
        Value = value;
    }
}