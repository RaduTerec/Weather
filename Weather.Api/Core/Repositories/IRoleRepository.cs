using Weather.Api.Core.Models;

namespace Weather.Api.Core.Repositories;

public interface IRoleRepository : IRepository<Role>
{
   Task<Role?> GetByNameAsync(string name);
}
