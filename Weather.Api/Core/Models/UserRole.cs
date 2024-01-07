using System.ComponentModel.DataAnnotations;

namespace Weather.Api.Core.Models;

public sealed class UserRole
{
   [Required]
   public User User { get; init; } = null!;
   public int UserId { get; init; }

   [Required]
   public Role Role { get; init; } = null!;
   public int RoleId { get; init; }
}
