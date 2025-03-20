using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities.Invokes;

public partial class Name : StringEnum
{
    public static readonly Name FileConsent = new("fileConsent/invoke");
    public bool IsFileConsent => FileConsent.Equals(Value);
}

public class FileConsentActivity() : InvokeActivity(Name.FileConsent)
{
    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public new FileConsentCardResponse? Value { get; set; }
}