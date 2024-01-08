using Weather.Api.Core.Models;

namespace Weather.Api.Core.Repositories;

public interface ICityRepository : IRepository<City>
{
   Task<bool> ExistsAsync(string name);
   Task<City?> GetWithMeasurements(int id);
}
