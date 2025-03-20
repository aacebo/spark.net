using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public partial class SignIn : StringEnum
    {
        public static readonly Name VerifyState = new("signin/verifyState");
        public bool IsVerifyState => VerifyState.Equals(Value);
    }
}

public static partial class SignIn
{
    public class VerifyStateActivity() : InvokeActivity(Name.SignIn.VerifyState)
    {
        /// <summary>
        /// A value that is associated with the activity.
        /// </summary>
        [JsonPropertyName("value")]
        [JsonPropertyOrder(32)]
        public new required Api.SignIn.StateVerifyQuery Value { get; set; }
    }
}