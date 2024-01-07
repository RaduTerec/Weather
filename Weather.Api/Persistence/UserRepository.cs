using Microsoft.EntityFrameworkCore;

using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;

namespace Weather.Api.Persistence;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
   private readonly WeatherDbContext _dbContext;

   public UserRepository(WeatherDbContext dbContext) : base(dbContext)
   {
      _dbContext = dbContext;
   }

   public async Task<User?> GetByEmailWithRolesAsync(string email)
   {
      return await _dbContext.Users
                             .Include(usr => usr.Roles)
                             .FirstOrDefaultAsync(usr => usr.Email == email);
   }
}
