using Weather.Api.Core.Models;
using Weather.Api.Persistence;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

internal static class DbSeeder
{
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
}
