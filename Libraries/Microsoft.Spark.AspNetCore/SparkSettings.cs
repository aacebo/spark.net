using Microsoft.Spark.Common.Logging;

namespace Microsoft.Spark.AspNetCore;

public class SparkSettings
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? TenantId { get; init; }
    public LoggingSettings Logging { get; init; } = new();
}