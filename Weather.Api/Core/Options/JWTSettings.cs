namespace Weather.Api.Core.Options;

/// <summary>
/// Reads JWT settings from the configuration file.
/// This class is initialized with safe default values.
/// </summary>
internal sealed class JWTSettings
{
   public string Key { get; init; } = "Just like that we can use this key in integration tests :)";
   public string Issuer { get; init; } = "WeatherApi";
   public string Audience { get; init; } = "WeatherUsers";
   public double DurationInMinutes { get; init; } = 90;
}
