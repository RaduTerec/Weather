using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Weather.Api.Core.DataTransferObjects;

public sealed class CityDTO
{
   public int Id { get; init; }

   [BindRequired]
   [MinLength(1)]
   [MaxLength(127)]
   [RegularExpression(@"^[^`~!@#$%^&*_|+=?;:'\""<>./{}\[\]]+$")]
   public string Name { get; init; } = string.Empty;
}
