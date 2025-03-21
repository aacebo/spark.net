using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api.Activities;

public partial class ActivityType : StringEnum
{
    public static readonly ActivityType Invoke = new("invoke");
    public bool IsInvoke => Invoke.Equals(Value);
}

public class InvokeActivity(Invokes.Name name) : Activity(ActivityType.Invoke)
{
    /// <summary>
    /// The name of the operation associated with an invoke or event activity.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(31)]
    public Invokes.Name Name { get; set; } = name;

    /// <summary>
    /// A value that is associated with the activity.
    /// </summary>
    [JsonPropertyName("value")]
    [JsonPropertyOrder(32)]
    public object? Value { get; set; }

    public Invokes.AdaptiveCardActivity ToAdaptiveCard() => (Invokes.AdaptiveCardActivity)this;
    public Invokes.ConfigActivity ToConfig() => (Invokes.ConfigActivity)this;
    public Invokes.MessageExtensionActivity ToMessageExtension() => (Invokes.MessageExtensionActivity)this;
    public Invokes.MessageActivity ToMessage() => (Invokes.MessageActivity)this;
    public Invokes.SignInActivity ToSignIn() => (Invokes.SignInActivity)this;
    public Invokes.TabActivity ToTab() => (Invokes.TabActivity)this;
    public Invokes.TaskActivity ToTask() => (Invokes.TaskActivity)this;
}