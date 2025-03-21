using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Configs
    {
        public static readonly Configs Submit = new("config/submit");
        public bool IsSubmit => Submit.Equals(Value);
    }
}

public static partial class Configs
{
    public class SubmitActivity : ConfigActivity
    {
        public SubmitActivity(object? value) : base(Name.Configs.Submit)
        {
            Value = value;
        }
    }
}