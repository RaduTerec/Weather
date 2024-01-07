using System.Net;
using System.Net.Http.Json;

using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Options;
using Weather.Integration.Tests.Common;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Endpoints;

[Collection("User integration tests")]
public sealed class UserEndpointsTests
   : IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
   private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
   private readonly RateLimitSettings _rateLimitSettings = new();
   private const string LoginPath = "/user/login";
   private const string RegisterPath = "/user/register";

   public UserEndpointsTests(IntegrationTestsWebApplicationFactory<Program> factory)
   {
      _factory = factory;
      _factory.SetupDatabaseAsync(DataOptions.Users).Wait();
   }

   [Fact]
   public async Task LoginAsync_ReturnsValidResponse()
   {
      var client = _factory.CreateClient();
      var loginDto = new LoginDTO { Email = FakeUserData.RegularUser.Email, Password = FakeUserData.RegularUser.Password };

      // Act
      var response = await client.PutAsJsonAsync(LoginPath, loginDto);
      HttpResponseMessage tooManyRequestsResponse = new();
      for (int i = 0; i <= _rateLimitSettings.LoginLimit; i++)
      {
         tooManyRequestsResponse = await client.PutAsJsonAsync(LoginPath, loginDto);
      }

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(HttpStatusCode.TooManyRequests, tooManyRequestsResponse.StatusCode);
   }

   [Fact]
   public async Task RegisterAsync_ReturnsValidResponse()
   {
      var client = _factory.CreateClient();
      var registerDto = new RegisterDTO { Email = "john@wth.com", Password = "How cool is this?", UserName = "johnSmith" };

      // Act
      var response = await client.PostAsJsonAsync(RegisterPath, registerDto);
      HttpResponseMessage tooManyRequestsResponse = new();
      for (int i = 0; i <= _rateLimitSettings.RegisterLimit; i++)
      {
         registerDto = new RegisterDTO { Email = $"user{i}@wth.com", Password = "What about this?!?", UserName = $"user{i}" };
         tooManyRequestsResponse = await client.PostAsJsonAsync(RegisterPath, registerDto);
      }

      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(HttpStatusCode.TooManyRequests, tooManyRequestsResponse.StatusCode);
   }
}
