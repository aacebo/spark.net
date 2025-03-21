using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.MessageExtensions;

/// <summary>
/// Messaging extension attachment.
/// </summary>
public class Attachment : Api.Attachment
{
    /// <summary>
    /// the preview attachment
    /// </summary>
    [JsonPropertyName("preview")]
    [JsonPropertyOrder(6)]
    public Api.Attachment? Preview { get; set; }

    public Attachment(string contentType) : base(contentType)
    {
    }

    public Attachment(ContentType contentType) : base(contentType)
    {
    }
}