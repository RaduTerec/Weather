using Weather.Api.Core.Models;

namespace Weather.Api.Core.Repositories;

public interface IUserRepository : IRepository<User>
{
   Task<User?> GetByEmailWithRolesAsync(string email);
}
