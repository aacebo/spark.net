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
    public ContentType ContentType { get; set; }

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

    public Attachment(string contentType)
    {
        ContentType = new(contentType);
    }

    public Attachment(ContentType contentType)
    {
        ContentType = contentType;
    }

    /// <summary>
    /// Attachment Layout
    /// </summary>
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
    public class AdaptiveCard(Card content) : Attachment(ContentType.AdaptiveCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Card Content { get; set; } = content;
    }

    /// <summary>
    /// AnimationCard Attachment
    /// </summary>
    public class AnimationCard(Cards.AnimationCard content) : Attachment(ContentType.AnimationCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.AnimationCard Content { get; set; } = content;
    }

    /// <summary>
    /// AudioCard Attachment
    /// </summary>
    public class AudioCard(Cards.AudioCard content) : Attachment(ContentType.AudioCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.AudioCard Content { get; set; } = content;
    }

    /// <summary>
    /// HeroCard Attachment
    /// </summary>
    public class HeroCard(Cards.HeroCard content) : Attachment(ContentType.HeroCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.HeroCard Content { get; set; } = content;
    }

    /// <summary>
    /// OAuthCard Attachment
    /// </summary>
    public class OAuthCard(Cards.OAuthCard content) : Attachment(ContentType.OAuthCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.OAuthCard Content { get; set; } = content;
    }

    /// <summary>
    /// SignInCard Attachment
    /// </summary>
    public class SignInCard(Cards.SignInCard content) : Attachment(ContentType.SignInCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.SignInCard Content { get; set; } = content;
    }

    /// <summary>
    /// ThumbnailCard Attachment
    /// </summary>
    public class ThumbnailCard(Cards.ThumbnailCard content) : Attachment(ContentType.ThumbnailCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.ThumbnailCard Content { get; set; } = content;
    }

    /// <summary>
    /// VideoCard Attachment
    /// </summary>
    public class VideoCard(Cards.VideoCard content) : Attachment(ContentType.VideoCard)
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.VideoCard Content { get; set; } = content;
    }
}