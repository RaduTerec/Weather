using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Weather.Api.Core.Models;

public sealed class User : IEntity
{
   public int Id { get; init; }

   [Required]
   [StringLength(255)]
   public string UserName { get; set; } = string.Empty;

   [Required]
   [StringLength(255)]
   [EmailAddress]
   public string Email { get; set; } = string.Empty;

   [Required]
   [StringLength(255)]
   public string PasswordHash { get; set; } = string.Empty;

   [Required]
   public ICollection<Role> Roles { get; set; } = new Collection<Role>();

   public ICollection<Measurement> Measurements { get; set; } = new Collection<Measurement>();
}
