using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly Name AnonQueryLink = new("composeExtension/anonymousQueryLink");
        public bool IsAnonQueryLink => AnonQueryLink.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class AnonQueryLinkActivity() : InvokeActivity(Name.MessageExtensions.AnonQueryLink)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required AppBasedQueryLink Value { get; set; }
    }
}