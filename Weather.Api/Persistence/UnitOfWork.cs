using Weather.Api.Core;
using Weather.Api.Core.Repositories;

namespace Weather.Api.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
   private readonly WeatherDbContext _dbContext;

   public ICityRepository Cities { get; }
   public IRoleRepository Roles { get; }
   public IUserRepository Users { get; }

   public UnitOfWork(WeatherDbContext dbContext)
   {
      _dbContext = dbContext;

      Cities = new CityRepository(_dbContext);
      Roles = new RoleRepository(_dbContext);
      Users = new UserRepository(_dbContext);
   }

   public async Task CommitAsync(CancellationToken cancellationToken = default)
   {
      await _dbContext.SaveChangesAsync(cancellationToken);
      // DbUpdateException is logged by the ErrorController
   }
}
