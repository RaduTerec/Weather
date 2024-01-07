using Weather.Api.Core;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeUnitOfWork : IUnitOfWork
{
   public IRoleRepository Roles { get; }
   public IUserRepository Users { get; }

   public FakeUnitOfWork(IRoleRepository roleRepository = null!,
                         IUserRepository userRepository = null!)
   {
      Roles = roleRepository;
      Users = userRepository;
   }

   public Task CommitAsync(CancellationToken cancellationToken = default)
   {
      return Task.CompletedTask;
   }
}
