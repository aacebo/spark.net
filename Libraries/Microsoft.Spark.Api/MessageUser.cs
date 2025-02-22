using System.Text.Json.Serialization;

namespace Microsoft.Spark.Api;

public class MessageUser
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    public required string Id { get; set; }

    [JsonPropertyName("displayName")]
    [JsonPropertyOrder(1)]
    public string? DisplayName { get; set; }

    [JsonPropertyName("userIdentityType")]
    [JsonPropertyOrder(2)]
    public UserIdentityType? Type { get; set; }
}

[JsonConverter(typeof(JsonConverter<UserIdentityType>))]
public class UserIdentityType(string value) : Common.StringEnum(value)
{
    public static readonly UserIdentityType AadUser = new("aadUser");
    public bool IsAadUser => AadUser.Equals(Value);

    public static readonly UserIdentityType OnPremiseAadUser = new("onPremiseAadUser");
    public bool IsOnPremiseAadUser => OnPremiseAadUser.Equals(Value);

    public static readonly UserIdentityType AnonymousGuest = new("anonymousGuest");
    public bool IsAnonymousGuest => AnonymousGuest.Equals(Value);

    public static readonly UserIdentityType FederatedUser = new("federatedUser");
    public bool IsFederatedUser => FederatedUser.Equals(Value);
}