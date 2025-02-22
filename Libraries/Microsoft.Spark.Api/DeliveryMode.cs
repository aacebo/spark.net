using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

[JsonConverter(typeof(JsonConverter<DeliveryMode>))]
public class DeliveryMode(string value) : StringEnum(value)
{
    public static readonly DeliveryMode Normal = new("normal");
    public bool IsNormal => Normal.Equals(Value);

    public static readonly DeliveryMode Notification = new("notification");
    public bool IsNotification => Notification.Equals(Value);

    public static readonly DeliveryMode ExpectReplies = new("expectReplies");
    public bool IsExpectReplies => ExpectReplies.Equals(Value);

    public static readonly DeliveryMode Ephemeral = new("ephemeral");
    public bool IsEphemeral => Ephemeral.Equals(Value);
}