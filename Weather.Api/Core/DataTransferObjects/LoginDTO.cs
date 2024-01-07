using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Weather.Api.Core.DataTransferObjects;

public sealed class LoginDTO
{
   [BindRequired]
   [EmailAddress]
   public string Email { get; init; } = string.Empty;

   [BindRequired]
   public string Password { get; init; } = string.Empty;
}
