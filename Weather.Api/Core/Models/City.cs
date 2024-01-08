using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Weather.Api.Core.Models;

public sealed class City : IEntity
{

   public int Id { get; init; }

   [Required]
   [StringLength(255)]
   public string Name { get; set; } = string.Empty;

   public ICollection<Measurement> Measurements { get; set; } = new Collection<Measurement>();
}
