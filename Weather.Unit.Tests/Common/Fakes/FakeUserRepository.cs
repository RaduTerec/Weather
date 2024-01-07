using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeUserRepository : FakeRepository<User>, IUserRepository
{
   public Task<User?> GetByEmailWithRolesAsync(string email)
      => Task.FromResult(this.FirstOrDefault(urs => urs.Email.Equals(email, StringComparison.Ordinal)));
}
