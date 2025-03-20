using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Configs
    {
        public static readonly Name Fetch = new("config/fetch");
        public bool IsFetch => Fetch.Equals(Value);
    }
}

public static partial class Configs
{
    public class FetchActivity : InvokeActivity
    {
        public FetchActivity(object? value) : base(Name.Configs.Fetch)
        {
            Value = value;
        }
    }
}