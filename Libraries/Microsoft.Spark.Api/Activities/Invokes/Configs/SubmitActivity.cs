using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Configs
    {
        public static readonly Name Submit = new("config/submit");
        public bool IsSubmit => Submit.Equals(Value);
    }
}

public static partial class Configs
{
    public class SubmitActivity : InvokeActivity
    {
        public SubmitActivity(object? value) : base(Name.Configs.Submit)
        {
            Value = value;
        }
    }
}