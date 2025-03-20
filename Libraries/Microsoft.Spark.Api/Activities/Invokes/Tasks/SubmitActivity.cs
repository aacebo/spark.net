using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Tasks : StringEnum
    {
        public static readonly Name Submit = new("task/submit");
        public bool IsSubmit => Submit.Equals(Value);
    }
}

public static partial class Tasks
{
    public class SubmitActivity() : InvokeActivity(Name.Tasks.Submit)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required TaskModules.Request Value { get; set; }
    }
}