using Weather.Api.Core.Models;

namespace Weather.Api.Core;

public interface IUserService
{
   string CreateJwtToken(User user, ICollection<Role> userRoles);
}
