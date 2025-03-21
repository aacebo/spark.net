using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly MessageExtensions FetchTask = new("composeExtension/fetchTask");
        public bool IsFetchTask => FetchTask.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class FetchTaskActivity() : MessageExtensionActivity(Name.MessageExtensions.FetchTask)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.MessageExtensions.Action Value { get; set; }
    }
}