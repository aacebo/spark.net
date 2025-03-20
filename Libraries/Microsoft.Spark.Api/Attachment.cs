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

    /// <summary>
    /// AnimationCard Attachment
    /// </summary>
    public class AnimationCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.AnimationCard Content { get; set; }

        public AnimationCard(Cards.AnimationCard content) : base()
        {
            ContentType = ContentType.AnimationCard;
            Content = content;
        }
    }

    /// <summary>
    /// AudioCard Attachment
    /// </summary>
    public class AudioCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.AudioCard Content { get; set; }

        public AudioCard(Cards.AudioCard content) : base()
        {
            ContentType = ContentType.AudioCard;
            Content = content;
        }
    }

    /// <summary>
    /// HeroCard Attachment
    /// </summary>
    public class HeroCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.HeroCard Content { get; set; }

        public HeroCard(Cards.HeroCard content) : base()
        {
            ContentType = ContentType.HeroCard;
            Content = content;
        }
    }

    /// <summary>
    /// OAuthCard Attachment
    /// </summary>
    public class OAuthCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.OAuthCard Content { get; set; }

        public OAuthCard(Cards.OAuthCard content) : base()
        {
            ContentType = ContentType.OAuthCard;
            Content = content;
        }
    }

    /// <summary>
    /// SignInCard Attachment
    /// </summary>
    public class SignInCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.SignInCard Content { get; set; }

        public SignInCard(Cards.SignInCard content) : base()
        {
            ContentType = ContentType.SignInCard;
            Content = content;
        }
    }

    /// <summary>
    /// ThumbnailCard Attachment
    /// </summary>
    public class ThumbnailCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.ThumbnailCard Content { get; set; }

        public ThumbnailCard(Cards.ThumbnailCard content) : base()
        {
            ContentType = ContentType.ThumbnailCard;
            Content = content;
        }
    }

    /// <summary>
    /// VideoCard Attachment
    /// </summary>
    public class VideoCard : Attachment
    {
        /// <summary>
        /// The Adaptive Card Content
        /// </summary>
        [JsonPropertyName("content")]
        [JsonPropertyOrder(4)]
        public new Cards.VideoCard Content { get; set; }

        public VideoCard(Cards.VideoCard content) : base()
        {
            ContentType = ContentType.VideoCard;
            Content = content;
        }
    }
}