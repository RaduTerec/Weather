using Weather.Api.Core;
using Weather.Api.Core.Models;

namespace Weather.Integration.Tests.Common.Models;

internal static class FakeUserData
{
   public static Role UserRole { get; } = new() { Name = Constants.User };
   public static Role AdminRole { get; } = new() { Name = Constants.Admin };

   public static FakeUser RegularUser { get; } = new(UserName: "regular", Email: "user@wth.com", Password: "Who are you really?", Roles: [UserRole]);

   public static FakeUser Admin { get; } = new(UserName: "admin", Email: "adm@wth.com", "I'm nobody. He he he!", Roles: [AdminRole]);
}
