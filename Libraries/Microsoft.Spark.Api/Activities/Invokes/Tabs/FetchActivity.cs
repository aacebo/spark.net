using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class Tabs : StringEnum
    {
        public static readonly Tabs Fetch = new("tab/fetch");
        public bool IsFetch => Fetch.Equals(Value);
    }
}

public static partial class Tabs
{
    public class FetchActivity() : TabInvokeActivity(Name.Tabs.Fetch)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.Tabs.Request Value { get; set; }
    }
}