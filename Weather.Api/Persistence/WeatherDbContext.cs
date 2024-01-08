using Microsoft.EntityFrameworkCore;

using Weather.Api.Core.Models;

namespace Weather.Api.Persistence;

internal sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
   public DbSet<User> Users { get; set; } = null!;
   public DbSet<Role> Roles { get; set; } = null!;

   protected override void OnModelCreating(ModelBuilder builder)
   {
      base.OnModelCreating(builder);

      // I don't want a random name for the many-to-many relation
      builder.Entity<User>()
             .HasMany(usr => usr.Roles)
             .WithMany(rle => rle.Users)
             .UsingEntity("UserRoles");
   }
}
