using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.AI.Messages;

/// <summary>
/// represents some message content
/// </summary>
public interface IContent
{
    /// <summary>
    /// the type of content
    /// </summary>
    public ContentType Type { get; }
}

public class UserMessage : IMessage
{
    [JsonPropertyName("role")]
    [JsonPropertyOrder(0)]
    public Role Role => Role.User;

    [JsonPropertyName("content")]
    [JsonPropertyOrder(1)]
    public required object Content { get; set; }

    public UserMessage(string content)
    {
        Content = content;
    }

    public UserMessage(IEnumerable<IContent> content)
    {
        Content = content;
    }

    public override string ToString()
    {
        if (Content is IEnumerable<IContent> asEnum)
        {
            return string.Join("\n", asEnum.Select(v => v.ToString()));
        }

        if (Content is string asString)
        {
            return asString;
        }

        return Content.ToString() ?? throw new InvalidCastException();
    }
}

[JsonConverter(typeof(JsonConverter<ContentType>))]
public class ContentType(string value) : StringEnum(value)
{
    public static readonly ContentType Text = new("text");
    public bool IsText => Text.Equals(Value);

    public static readonly ContentType ImageUrl = new("image_url");
    public bool IsImageUrl => ImageUrl.Equals(Value);
}

public class TextContent : IContent
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(0)]
    public ContentType Type => ContentType.Text;

    [JsonPropertyName("text")]
    [JsonPropertyOrder(1)]
    public required string Text { get; set; }

    public override string ToString() => Text;
}

public class ImageContent : IContent
{
    [JsonPropertyName("type")]
    [JsonPropertyOrder(0)]
    public ContentType Type => ContentType.ImageUrl;

    [JsonPropertyName("image_url")]
    [JsonPropertyOrder(1)]
    public required string ImageUrl { get; set; }

    public override string ToString() => ImageUrl;
}