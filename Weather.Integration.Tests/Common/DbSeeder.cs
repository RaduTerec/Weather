using Weather.Api.Core.Models;
using Weather.Api.Persistence;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

internal static class DbSeeder
{
   public static int NumberOfCitiesInDb { get; private set; }

   public static async Task SeedValuesAsync(WeatherDbContext context, DataOptions options)
   {
      if (options == DataOptions.None)
      {
         return;
      }

      try
      {
         await context.Roles.AddRangeAsync(SeedRoles());
         await context.SaveChangesAsync();
         await context.Users.AddRangeAsync(SeedUsers(context));

         switch (options)
         {
            case DataOptions.Cities:
               var cities = SeedCities();
               NumberOfCitiesInDb = cities.Length;
               await context.Cities.AddRangeAsync(cities);
               await context.SaveChangesAsync();
               break;
         }

         await context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.Error.Write(ex.Message);
         Console.Error.Write(ex.StackTrace);
      }
   }

   private static Role[] SeedRoles()
    => [FakeUserData.UserRole, FakeUserData.AdminRole];

   private static User[] SeedUsers(WeatherDbContext context)
    => [FakeUserData.Admin.ToUser(context), FakeUserData.RegularUser.ToUser(context)];

   public static City[] SeedCities()
      => [
         new City { Name = "Zürich" },
         new City { Name = "Bern" },
         new City { Name = "Berlin" },
         new City { Name = "Stuttgart" },
         new City { Name = "Manilla" },
      ];
}
