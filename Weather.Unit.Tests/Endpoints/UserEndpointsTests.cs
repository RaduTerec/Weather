using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;
using Weather.Api.Endpoints;
using Weather.Unit.Tests.Common;
using Weather.Unit.Tests.Common.Fakes;

namespace Weather.Unit.Tests.Endpoints;

public sealed class UserEndpointsTests
{
   private readonly IUserRepository _userRepository;

   public UserEndpointsTests()
   {
      _userRepository = new FakeUserRepository();
   }

   [Fact]
   public async Task LoginAsync_ReturnsBadRequest_WhenNoUser()
   {
      var loginAttempt = new LoginDTO();
      var unitOfWork = new FakeUnitOfWork(userRepository: _userRepository);

      // Act
      var result = await UserEndpoints.LoginAsync(loginAttempt, unitOfWork, new FakePasswordHasher<User>(), new FakeUserService());
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("Invalid user data", problemDetail.Detail);
   }

   [Fact]
   public async Task LoginAsync_ReturnsBadRequest_WhenInvalidCredentials()
   {
      var login = new LoginDTO { Email = "user@wth.com", Password = "wrongPass" };
      var user = new User { Email = login.Email, PasswordHash = "uth2897asd2sjo20" };
      await _userRepository.AddAsync(user);
      var unitOfWork = new FakeUnitOfWork(userRepository: _userRepository);

      // Act
      var result = await UserEndpoints.LoginAsync(login, unitOfWork, new FakePasswordHasher<User>(), new FakeUserService());
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("Invalid user data", problemDetail.Detail);
   }


   [Fact]
   public async Task LoginAsync_ReturnsBadRequest_WhenUserAlreadyExists()
   {
      var register = new RegisterDTO { Email = "user@wth.com" };
      var user = new User { Email = register.Email };
      await _userRepository.AddAsync(user);
      var unitOfWork = new FakeUnitOfWork(userRepository: _userRepository);

      // Act
      var result = await UserEndpoints.RegisterAsync(register, unitOfWork, new FakePasswordHasher<User>(), new FakeUserService());
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("Invalid user data", problemDetail.Detail);
   }
}
