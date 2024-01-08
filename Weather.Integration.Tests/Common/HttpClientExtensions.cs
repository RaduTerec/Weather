using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Http;

using Weather.Api.Core.DataTransferObjects;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

internal static class HttpClientExtensions
{
   private static readonly Dictionary<string, string> _tokenStore = new(StringComparer.Ordinal);
   private static readonly JsonSerializerOptions _serializerOptions = new()
   {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
   };

   public static async Task<HttpResponseMessage> AuthenticatedJsonPostAsync<T>(this HttpClient httpClient, string requestUri, T valueToSerialize, string token)
   {
      var serializedObject = JsonSerializer.Serialize(valueToSerialize, _serializerOptions);

      var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
      {
         Content = new StringContent(serializedObject, Encoding.UTF8, "application/json"),
      };
      request.Headers.Add(nameof(HttpRequestHeader.Authorization), $"Bearer {token}");

      return await httpClient.SendAsync(request);
   }

   public static async Task<HttpResponseMessage> AuthenticatedJsonPutAsync<T>(this HttpClient httpClient, string requestUri, T valueToSerialize, string token)
   {
      var serializedObject = JsonSerializer.Serialize(valueToSerialize, _serializerOptions);

      var request = new HttpRequestMessage(HttpMethod.Put, requestUri)
      {
         Content = new StringContent(serializedObject, Encoding.UTF8, "application/json"),
      };
      request.Headers.Add(nameof(HttpRequestHeader.Authorization), $"Bearer {token}");

      return await httpClient.SendAsync(request);
   }

   public static async Task<HttpResponseMessage> AuthenticatedDeleteAsync(this HttpClient httpClient, string requestUri, string token)
   {
      var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
      request.Headers.Add(nameof(HttpRequestHeader.Authorization), $"Bearer {token}");

      return await httpClient.SendAsync(request);
   }

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
