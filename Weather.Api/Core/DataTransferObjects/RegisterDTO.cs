using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Weather.Api.Core.DataTransferObjects;

public sealed class RegisterDTO
{
   [BindRequired]
   [EmailAddress]
   public string Email { get; init; } = string.Empty;

   [BindRequired]
   public string Password { get; init; } = string.Empty;

   [BindRequired]
   [MinLength(1)]
   [MaxLength(127)]
   [RegularExpression(@"^[^`~!@#$%^&*_|+=?;:'\""<>./{}\[\]]+$")]
   public string UserName { get; init; } = string.Empty;
}
