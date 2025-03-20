using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly Name SubmitAction = new("composeExtension/submitAction");
        public bool IsSubmitAction => SubmitAction.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class SubmitActionActivity() : InvokeActivity(Name.MessageExtensions.SubmitAction)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.MessageExtensions.Action Value { get; set; }
    }
}