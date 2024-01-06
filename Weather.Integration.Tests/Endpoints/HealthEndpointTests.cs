using System.Net.Http.Json;

using Weather.Integration.Tests.Common;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Endpoints;

[Collection("Health integration tests")]
public sealed class HealthEndpointTests
   : IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
   private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

   public HealthEndpointTests(IntegrationTestsWebApplicationFactory<Program> factory)
   {
      _factory = factory;
      _factory.SetupDatabaseAsync(DataOptions.None).Wait();
   }

   [Fact]
   public async Task HealthCheck_ReturnsOk()
   {
      var client = _factory.CreateClient();

      // Act
      var response = await client.GetFromJsonAsync<HealthDto>("/health");

      Assert.Equal("Healthy", response!.Status);
      foreach (var entry in response.Results)
      {
         Assert.NotEmpty(entry.Name);
         Assert.Equal("Healthy", entry.Status);
      }
   }
}
