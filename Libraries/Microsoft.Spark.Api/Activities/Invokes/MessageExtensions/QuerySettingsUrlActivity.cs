using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class MessageExtensions : StringEnum
    {
        public static readonly Name QuerySettingsUrl = new("composeExtension/querySettingsUrl");
        public bool IsQuerySettingsUrl => QuerySettingsUrl.Equals(Value);
    }
}

public static partial class MessageExtensions
{
    public class QuerySettingsUrlActivity() : InvokeActivity(Name.MessageExtensions.QuerySettingsUrl)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.MessageExtensions.Query Value { get; set; }
    }
}