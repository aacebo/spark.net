using Microsoft.Extensions.Configuration;

namespace Microsoft.Spark.AspNetCore;

public record SparkSettings(string ClientId, string? ClientSecret, string? TenantId)
{
    public string ClientId { get; } = ClientId;
    public string? ClientSecret { get; } = ClientSecret;
    public string? TenantId { get; } = TenantId;
};

public static class ConfigurationExtensions
{
    public static SparkSettings? GetSpark(this IConfiguration configuration)
    {
        var section = configuration.GetSection("Spark");

        if (section == null)
            return null;

        var clientId = section.GetValue<string>("ClientId");

        if (clientId == null)
            return null;

        var clientSecret = section.GetValue<string>("ClientSecret");
        var tenantId = section.GetValue<string>("TenantId");
        return new SparkSettings(clientId, clientSecret, tenantId);
    }
}