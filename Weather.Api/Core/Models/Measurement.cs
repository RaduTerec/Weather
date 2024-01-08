using System.ComponentModel.DataAnnotations;

namespace Weather.Api.Core.Models;

public sealed class Measurement : IEntity
{

   public int Id { get; init; }

   [Required]
   [Range(-273.15, 999999)]
   public float Temperature { get; set; }

   [Required]
   [ConcurrencyCheck]
   public DateTime Timestamp { get; set; }

   [Required]
   public User User { get; set; } = null!;
   public int UserId { get; set; }

   [Required]
   public City City { get; set; } = null!;
   public int CityId { get; set; }
}
