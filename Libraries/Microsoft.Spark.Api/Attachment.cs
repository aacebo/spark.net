using System.Text.Json.Serialization;

using Microsoft.Spark.Common;

namespace Microsoft.Spark.Api;

public class Attachment
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public string? Id { get; set; }

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
