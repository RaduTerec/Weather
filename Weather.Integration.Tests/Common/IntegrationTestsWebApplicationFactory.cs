using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Weather.Api.Persistence;
using Weather.Integration.Tests.Common.Models;

// Set the orderer
[assembly: TestCollectionOrderer("Weather.Integration.Tests.XUnit.CollectionOrderer", "Weather.Integration.Tests")]

// Need to turn off test parallelization so we can validate the run order
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Weather.Integration.Tests.Common;

public sealed class IntegrationTestsWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram>
    where TProgram : class
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureServices(RemoveRealServices);
      builder.ConfigureTestServices(ConfigureIntegrationDatabase);
      builder.UseEnvironment("Development");
   }

   private static void RemoveRealServices(IServiceCollection services)
   {
      var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<WeatherDbContext>));
      if (dbContextDescriptor != default)
      {
         services.Remove(dbContextDescriptor);
      }
   }

   private static void ConfigureIntegrationDatabase(IServiceCollection services)
   {
      services.AddSingleton<DbConnection>(_ =>
      {
         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();

         return connection;
      });

      services.AddDbContextPool<WeatherDbContext>((container, options) =>
      {
         var connection = container.GetRequiredService<DbConnection>();
         options.UseSqlite(connection);
      });
   }

   public async Task SetupDatabaseAsync(DataOptions options)
   {
      var scope = this.Services.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
      await dbContext.Database.EnsureCreatedAsync();
      if (!dbContext.Users.Any() && options != DataOptions.None)
      {
         await DbSeeder.SeedValuesAsync(dbContext, options);
      }
   }

   public override async ValueTask DisposeAsync()
   {
      var scope = this.Services.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
      var connection = scope.ServiceProvider.GetRequiredService<DbConnection>();
      await dbContext.Database.EnsureDeletedAsync();
      await connection.DisposeAsync();
      await base.DisposeAsync();
   }
}
