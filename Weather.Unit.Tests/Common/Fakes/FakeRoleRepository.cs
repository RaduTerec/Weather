using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeRoleRepository : FakeRepository<Role>, IRoleRepository
{
   public Task<Role?> GetByNameAsync(string name)
      => Task.FromResult(this.FirstOrDefault(rl => rl.Name.Equals(name, StringComparison.Ordinal)));
}
