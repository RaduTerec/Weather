using System.Net.Http.Json;

using Microsoft.Extensions.DependencyInjection;

using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Options;
using Weather.Api.Persistence;
using Weather.Integration.Tests.Common;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Endpoints;

[Collection("City integration tests")]
public sealed class CityEndpointsTests
   : IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
   private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
   private const string CityPath = "/city/";

   public CityEndpointsTests(IntegrationTestsWebApplicationFactory<Program> factory)
   {
      _factory = factory;
      _factory.SetupDatabaseAsync(DataOptions.Cities).Wait();
   }

   [Fact]
   public async Task GetAsync_ReturnsData()
   {
      var client = _factory.CreateClient();
      var scope = _factory.Services.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
      int cityId = dbContext.Cities.First(cty => cty.Name == "Bern").Id;

      // Act
      var city = await client.GetFromJsonAsync<CityDTO>($"{CityPath}{cityId}");

      Assert.Equal(cityId, city!.Id);
      Assert.NotEmpty(city.Name);
   }

   [Fact]
   public async Task InsertAsync_ReturnsOk_WhenAdminInsertsData()
   {
      int numberOfCitiesInDb = DbSeeder.NumberOfCitiesInDb;
      var client = _factory.CreateClient();
      var cityDto = new CityDTO { Name = "Cluj" };

      // Act
      string token = await client.GetAuthenticationToken(FakeUserData.Admin);
      var cityResponse = await client.AuthenticatedJsonPostAsync(CityPath, cityDto, token);
      var newCityDto = await cityResponse.Content.ReadFromJsonAsync<CityDTO>();

      Assert.True(newCityDto!.Id > numberOfCitiesInDb,
                  $"newCityDto.Id should be greater than {numberOfCitiesInDb}");
      Assert.Equal(cityDto.Name, newCityDto.Name);
   }

   [Fact]
   public async Task UpdateAsync_ReturnsOk_WhenAdminUpdatesData()
   {
      var client = _factory.CreateClient();
      var scope = _factory.Services.CreateAsyncScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
      int cityId = dbContext.Cities.First(cty => cty.Name == "Zürich").Id;
      var updatedCity = new CityDTO { Id = cityId, Name = "Züri" };

      // Act
      string token = await client.GetAuthenticationToken(FakeUserData.Admin);
      var cityResponse = await client.AuthenticatedJsonPutAsync($"{CityPath}{cityId}", updatedCity, token);
      var updatedCityDto = await cityResponse.Content.ReadFromJsonAsync<CityDTO>();

      Assert.Equal(updatedCity.Id, updatedCityDto!.Id);
      Assert.Equal(updatedCity.Name, updatedCityDto.Name);
   }

   [Fact]
   public async Task DeleteAsync_ReturnsOk_WhenAdminDeletesData()
   {
      var client = _factory.CreateClient();
      // I cannot delete a city if it has measurements associated with it
      var cityDto = new CityDTO { Name = "Basel" };

      // Act
      string token = await client.GetAuthenticationToken(FakeUserData.Admin);
      var cityResponse = await client.AuthenticatedJsonPostAsync(CityPath, cityDto, token);
      var newCityDto = await cityResponse.Content.ReadFromJsonAsync<CityDTO>();
      var deletedCityResponse = await client.AuthenticatedDeleteAsync($"{CityPath}{newCityDto!.Id}", token);
      var deletedCityId = await deletedCityResponse.Content.ReadFromJsonAsync<int>();

      Assert.Equal(newCityDto!.Id, deletedCityId);
   }
}
