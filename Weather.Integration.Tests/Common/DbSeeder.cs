using Weather.Api.Persistence;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

internal static class DbSeeder
{
   public static async Task SeedDatabaseAsync(WeatherDbContext context, DataOptions options)
   {
      try
      {
         if (options == DataOptions.None)
         {
            return;
         }

         await context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.Error.Write(ex.Message);
         Console.Error.Write(ex.StackTrace);
      }
   }
}
