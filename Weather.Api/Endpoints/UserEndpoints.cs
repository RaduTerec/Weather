using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Identity;

using Weather.Api.Core;
using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Models;

namespace Weather.Api.Endpoints;

public static class UserEndpoints
{
   public static RouteGroupBuilder ToUserEndpoints(this RouteGroupBuilder userItems)
   {
      userItems.MapPut("/login", LoginAsync).RequireRateLimiting(Constants.LoginPolicy);
      userItems.MapPost("/register", RegisterAsync).RequireRateLimiting(Constants.RegisterPolicy);

      return userItems;
   }

   public static async Task<IResult> LoginAsync(LoginDTO login, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
   {
      var user = await unitOfWork.Users.GetByEmailWithRolesAsync(login.Email);
      if (user == default)
      {
         return TypedResults.Problem(detail: "Invalid user data", statusCode: StatusCodes.Status400BadRequest);
      }

      var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);
      if (result != PasswordVerificationResult.Success && result != PasswordVerificationResult.SuccessRehashNeeded)
      {
         return TypedResults.Problem(detail: "Invalid user data", statusCode: StatusCodes.Status400BadRequest);
      }

      // will be replaced with a JWT token
      return TypedResults.Ok();
   }

   public static async Task<IResult> RegisterAsync(RegisterDTO register, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
   {
      var user = await unitOfWork.Users.GetByEmailWithRolesAsync(register.Email);
      if (user != default)
      {
         return TypedResults.Problem(detail: "Invalid user data", statusCode: StatusCodes.Status400BadRequest);
      }

      string passwordHash = passwordHasher.HashPassword(new User(), register.Password);
      var userRole = await unitOfWork.Roles.GetByNameAsync(Constants.User);

      var newUser = new User() { Email = register.Email, UserName = register.UserName, PasswordHash = passwordHash };
      newUser.Roles = new Collection<UserRole> { new() { UserId = newUser.Id, RoleId = userRole!.Id } };

      await unitOfWork.Users.AddAsync(newUser);
      await unitOfWork.CommitAsync();

      // will be replaced with a JWT token
      return TypedResults.Ok();
   }
}
