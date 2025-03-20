using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly MessageExtensions Query = new("composeExtension/query");
        public bool IsQuery => Query.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class QueryActivity() : MessageExtensionInvokeActivity(Name.MessageExtensions.Query)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.MessageExtensions.Query Value { get; set; }
    }
}