using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class MessageReaction
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(0)]
    public required MessageReactionType Type { get; set; }

    [JsonPropertyName("user")]
    [JsonPropertyOrder(1)]
    public MessageUser? User { get; set; }

    [JsonPropertyName("createdDateTime")]
    [JsonPropertyOrder(2)]
    public DateTime? CreatedDateTime { get; set; }
}

[JsonConverter(typeof(JsonConverter<MessageReactionType>))]
public class MessageReactionType(string value) : Common.StringEnum(value)
{
    public static readonly MessageReactionType Like = new("like");
    public bool IsLike => Like.Equals(Value);

    public static readonly MessageReactionType Heart = new("heart");
    public bool IsHeart => Heart.Equals(Value);

    public static readonly MessageReactionType Laugh = new("laugh");
    public bool IsLaugh => Laugh.Equals(Value);

    public static readonly MessageReactionType Surprised = new("surprised");
    public bool IsSurprised => Surprised.Equals(Value);

    public static readonly MessageReactionType Sad = new("sad");
    public bool IsSad => Sad.Equals(Value);

    public static readonly MessageReactionType Angry = new("angry");
    public bool IsAngry => Angry.Equals(Value);

    public static readonly MessageReactionType PlusOne = new("plusOne");
    public bool IsPlusOne => PlusOne.Equals(Value);
}