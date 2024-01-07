using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Weather.Api.Core.DataTransferObjects;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

internal static class HttpClientExtensions
{
   private static readonly Dictionary<string, string> _tokenStore = new(StringComparer.Ordinal);

   public static async Task<string> GetAuthenticationToken(this HttpClient client, FakeUser user)
   {
      if (_tokenStore.TryGetValue(user.Email, out string? token))
      {
         return token;
      }

      var loginDto = new LoginDTO { Email = user.Email, Password = user.Password };
      var loginResponse = await client.PutAsJsonAsync("/user/login", loginDto);
      var authenticationResponseDto = await loginResponse.Content.ReadFromJsonAsync<AuthenticationResponseDto>();
      _tokenStore.Add(user.Email, authenticationResponseDto!.Token);

      return authenticationResponseDto.Token;
   }
}
