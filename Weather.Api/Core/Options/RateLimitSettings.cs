namespace Weather.Api.Core.Options;

/// <summary>
/// Reads the rate limiting settings from the configuration file.
/// This class is initialized with safe default values.
/// </summary>
public sealed class RateLimitSettings
{
   public int LoginLimit { get; set; } = 10;
   public int LoginWindow { get; set; } = 1;
   public int RegisterLimit { get; set; } = 1;
   public int GlobalLimit { get; set; } = 100;
   public int Window { get; set; } = 1;
}
