using Weather.Api.Core.Repositories;

namespace Weather.Api.Core;

public interface IUnitOfWork
{
   ICityRepository Cities { get; }
   IRoleRepository Roles { get; }
   IUserRepository Users { get; }
   Task CommitAsync(CancellationToken cancellationToken = default);
}
