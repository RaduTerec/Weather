using Microsoft.AspNetCore.Identity;

using Weather.Api.Core.Models;
using Weather.Api.Persistence;
using Weather.Integration.Tests.Common.Models;

namespace Weather.Integration.Tests.Common;

public static class UserExtensions
{
   private static readonly PasswordHasher<User> _hasher = new();

   internal static User ToUser(this FakeUser fakeUser, WeatherDbContext context)
      => new()
      {
         Email = fakeUser.Email,
         UserName = fakeUser.UserName,
         PasswordHash = _hasher.HashPassword(new User(), fakeUser.Password),
         Roles = [context.Roles.First(rl => rl.Name == fakeUser.Roles[0].Name)],
      };
}
