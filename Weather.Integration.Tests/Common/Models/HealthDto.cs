namespace Weather.Integration.Tests.Common.Models;

public sealed record HealthDto(string Status, IEnumerable<Entry> Results);

public sealed record Entry(string Name, string Status);
