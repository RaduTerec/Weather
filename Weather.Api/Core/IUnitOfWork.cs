using Weather.Api.Core.Repositories;

namespace Weather.Api.Core;

public interface IUnitOfWork
{
   IRoleRepository Roles { get; }
   IUserRepository Users { get; }
   Task CommitAsync(CancellationToken cancellationToken = default);
}
