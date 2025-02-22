using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api.Entities;

public interface IClientInfoEntity : IEntity
{
    [JsonPropertyName("locale")]
    [JsonPropertyOrder(3)]
    public string Locale { get; set; }

    [JsonPropertyName("country")]
    [JsonPropertyOrder(4)]
    public string Country { get; set; }

    [JsonPropertyName("platform")]
    [JsonPropertyOrder(5)]
    public string Platform { get; set; }

    [JsonPropertyName("timezone")]
    [JsonPropertyOrder(6)]
    public string Timezone { get; set; }
}

public class ClientInfoEntity : Entity, IClientInfoEntity
{
    [JsonPropertyName("locale")]
    [JsonPropertyOrder(3)]
    public required string Locale { get; set; }

    [JsonPropertyName("country")]
    [JsonPropertyOrder(4)]
    public required string Country { get; set; }

    [JsonPropertyName("platform")]
    [JsonPropertyOrder(5)]
    public required string Platform { get; set; }

    [JsonPropertyName("timezone")]
    [JsonPropertyOrder(6)]
    public required string Timezone { get; set; }

    public ClientInfoEntity() : base("clientInfo") { }
}