using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Weather.Api.Core.Models;

public sealed class Role : IEntity
{
   public int Id { get; init; }

   [Required]
   [StringLength(255)]
   public string Name { get; init; } = string.Empty;

   public ICollection<User> Users { get; set; } = new Collection<User>();
}
