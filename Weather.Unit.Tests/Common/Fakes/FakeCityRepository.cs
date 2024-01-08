using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeCityRepository : FakeRepository<City>, ICityRepository
{
   public Task<bool> ExistsAsync(string name)
   {
      var exists = this.Any(cty => string.Equals(cty.Name, name, StringComparison.Ordinal));
      return Task.FromResult(exists);
   }

   public Task<City?> GetWithMeasurements(int id) => this.GetAsync(id);
}
