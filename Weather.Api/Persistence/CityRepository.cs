using Microsoft.EntityFrameworkCore;

using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Api.Persistence;

internal sealed class CityRepository : Repository<City>, ICityRepository
{
   private readonly WeatherDbContext _dbContext;

   public CityRepository(WeatherDbContext dbContext) : base(dbContext)
   {
      _dbContext = dbContext;
   }

   public async Task<bool> ExistsAsync(string name)
   {
      return await _dbContext.Cities.AnyAsync(cty => cty.Name == name);
   }

   public async Task<City?> GetWithMeasurements(int id)
   {
      return await _dbContext.Cities.Include(cty => cty.Measurements).FirstOrDefaultAsync(cty => cty.Id == id);
   }
}
