using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public static readonly Name ConfigSubmit = new("config/submit");
    public bool IsConfigSubmit => ConfigSubmit.Equals(Value);
}

public class ConfigSubmitActivity : InvokeActivity
{
    public ConfigSubmitActivity(object? value) : base(Name.ConfigFetch)
    {
        Value = value;
    }
}