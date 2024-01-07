using Microsoft.AspNetCore.Identity;

using Weather.Api.Core.Models;

namespace Weather.Unit.Tests.Common.Fakes;

internal sealed class FakePasswordHasher<TUser> : IPasswordHasher<TUser>
   where TUser : class, IEntity
{
   public string HashPassword(TUser user, string password) => throw new NotImplementedException();

   public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    => hashedPassword.Equals(providedPassword, StringComparison.Ordinal)
         ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
}
