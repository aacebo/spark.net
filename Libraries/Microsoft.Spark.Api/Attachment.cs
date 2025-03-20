using System.Text.Json.Serialization;

using Microsoft.Spark.Cards;
using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

/// <summary>
/// Attachment
/// </summary>
public class Attachment
{
    /// <summary>
    /// The id of the attachment.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string? Id { get; set; }

    /// <summary>
    /// (OPTIONAL) The name of the attachment
    /// </summary>
    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }

    /// <summary>
    /// mimetype/Contenttype for the file
    /// </summary>
    [JsonPropertyName("contentType")]
    [JsonPropertyOrder(2)]
    public required ContentType ContentType { get; set; }

    /// <summary>
    /// Content Url
    /// </summary>
    [JsonPropertyName("contentUrl")]
    [JsonPropertyOrder(3)]
    public string? ContentUrl { get; set; }

    /// <summary>
    /// Embedded content
    /// </summary>
    [JsonPropertyName("content")]
    [JsonPropertyOrder(4)]
    public object? Content { get; set; }

    /// <summary>
    /// (OPTIONAL) Thumbnail associated with attachment
    /// </summary>
    [JsonPropertyName("thumbnailUrl")]
    [JsonPropertyOrder(5)]
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Attachment Layout
    /// </summary>
    /// <param name="value"></param>
    public class Layout(string value) : StringEnum(value)
    {
        public static readonly Layout List = new("list");
        public bool IsList => List.Equals(Value);

        public static readonly Layout Carousel = new("carousel");
        public bool IsCarousel => Carousel.Equals(Value);
    }

    /// <summary>
    /// AdaptiveCard Attachment
    /// </summary>
    public class AdaptiveCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Card Content { get; set; }

        public AdaptiveCard(Card content) : base()
        {
            ContentType = ContentType.AdaptiveCard;
            Content = content;
        }
    }
}