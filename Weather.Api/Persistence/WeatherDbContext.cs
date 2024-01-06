using Microsoft.EntityFrameworkCore;

namespace Weather.Api.Persistence;

internal sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
}
