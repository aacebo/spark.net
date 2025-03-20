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

    public Invokes.AdaptiveCardInvokeActivity ToAdaptiveCard()
    {
        return (Invokes.AdaptiveCardInvokeActivity)this;
    }

    public Invokes.ConfigInvokeActivity ToConfig()
    {
        return (Invokes.ConfigInvokeActivity)this;
    }

    public Invokes.MessageExtensionInvokeActivity ToMessageExtension()
    {
        return (Invokes.MessageExtensionInvokeActivity)this;
    }

    public Invokes.MessageInvokeActivity ToMessage()
    {
        return (Invokes.MessageInvokeActivity)this;
    }

    public Invokes.SignInInvokeActivity ToSignIn()
    {
        return (Invokes.SignInInvokeActivity)this;
    }

    public Invokes.TabInvokeActivity ToTab()
    {
        return (Invokes.TabInvokeActivity)this;
    }

    public Invokes.TaskInvokeActivity ToTask()
    {
        return (Invokes.TaskInvokeActivity)this;
    }
}