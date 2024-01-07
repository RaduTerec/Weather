using Microsoft.EntityFrameworkCore;

using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Api.Persistence;

internal sealed class RoleRepository : Repository<Role>, IRoleRepository
{
   private readonly WeatherDbContext _dbContext;

   public RoleRepository(WeatherDbContext dbContext) : base(dbContext)
   {
      _dbContext = dbContext;
   }

   public async Task<Role?> GetByNameAsync(string name)
   {
      return await _dbContext.Roles
                             .FirstOrDefaultAsync(rl => rl.Name == name);
   }
}
