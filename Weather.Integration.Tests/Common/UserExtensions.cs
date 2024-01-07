using Microsoft.AspNetCore.Identity;

using Weather.Api.Core.Models;
using Weather.Api.Persistence;

namespace Weather.Integration.Tests.Common;

public static class UserExtensions
{
   private static readonly PasswordHasher<User> _hasher = new();

   internal static User ToUser(this FakeUser fakeUser)
      => new()
      {
         Email = fakeUser.Email,
         UserName = fakeUser.UserName,
         PasswordHash = _hasher.HashPassword(new User(), fakeUser.Password),
      };

   internal static UserRole ToUserRole(this FakeUser fakeUser, WeatherDbContext context)
      => new()
      {
         User = context.Users.First(us => us.Email == fakeUser.Email),
         Role = context.Roles.First(rl => rl.Name == fakeUser.Roles.First().Name)
      };
}
