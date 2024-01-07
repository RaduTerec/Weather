using Weather.Api.Core;
using Weather.Api.Core.Models;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeUserService(string authorization = "") : IUserService
{
   public string CreateJwtToken(User user, ICollection<Role> userRoles)
      => authorization;
}
