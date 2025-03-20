using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Tasks : StringEnum
    {
        public static readonly Tasks Fetch = new("task/fetch");
        public bool IsFetch => Fetch.Equals(Value);
    }
}

public static partial class Tasks
{
    public class FetchActivity() : TaskInvokeActivity(Name.Tasks.Fetch)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required TaskModules.Request Value { get; set; }
    }
}