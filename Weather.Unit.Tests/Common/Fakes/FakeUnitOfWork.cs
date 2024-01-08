using Weather.Api.Core;
using Weather.Api.Core.Repositories;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakeUnitOfWork : IUnitOfWork
{
   public ICityRepository Cities { get; }
   public IRoleRepository Roles { get; }
   public IUserRepository Users { get; }

   public FakeUnitOfWork(ICityRepository cityRepository = null!,
                         IRoleRepository roleRepository = null!,
                         IUserRepository userRepository = null!)
   {
      Cities = cityRepository;
      Roles = roleRepository;
      Users = userRepository;
   }

   public Task CommitAsync(CancellationToken cancellationToken = default)
   {
      return Task.CompletedTask;
   }
}
