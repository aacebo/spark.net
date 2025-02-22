using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

public class Attachment
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }

    [JsonPropertyName("contentType")]
    [JsonPropertyOrder(2)]
    public required ContentType ContentType { get; set; }

    [JsonPropertyName("contentUrl")]
    [JsonPropertyOrder(3)]
    public string? ContentUrl { get; set; }

    [JsonPropertyName("content")]
    [JsonPropertyOrder(4)]
    public string? Content { get; set; }

    [JsonPropertyName("thumbnailUrl")]
    [JsonPropertyOrder(5)]
    public string? ThumbnailUrl { get; set; }

    public class Layout(string value) : StringEnum(value)
    {
        public static readonly Layout List = new("list");
        public bool IsList => List.Equals(Value);

        public static readonly Layout Carousel = new("carousel");
        public bool IsCarousel => Carousel.Equals(Value);
    }
}

[JsonConverter(typeof(JsonConverter<ContentType>))]
public class ContentType(string value) : StringEnum(value)
{
    public static readonly ContentType AdaptiveCard = new("application/vnd.microsoft.card.adaptive");
    public bool IsAdaptiveCard => AdaptiveCard.Equals(Value);

    public static readonly ContentType AnimationCard = new("application/vnd.microsoft.card.animation");
    public bool IsAnimationCard => AnimationCard.Equals(Value);

    public static readonly ContentType AudioCard = new("application/vnd.microsoft.card.audio");
    public bool IsAudioCard => AudioCard.Equals(Value);

    public static readonly ContentType HeroCard = new("application/vnd.microsoft.card.hero");
    public bool IsHeroCard => HeroCard.Equals(Value);

    public static readonly ContentType OAuthCard = new("application/vnd.microsoft.card.oauth");
    public bool IsOAuthCard => OAuthCard.Equals(Value);

    public static readonly ContentType SignInCard = new("application/vnd.microsoft.card.signin");
    public bool IsSignInCard => SignInCard.Equals(Value);

    public static readonly ContentType ThumbnailCard = new("application/vnd.microsoft.card.thumbnail");
    public bool IsThumbnailCard => ThumbnailCard.Equals(Value);

    public static readonly ContentType VideoCard = new("application/vnd.microsoft.card.video");
    public bool IsVideoCard => VideoCard.Equals(Value);
}