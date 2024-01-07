using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Weather.Api.Core;
using Weather.Api.Core.Models;
using Weather.Api.Core.Options;

namespace Weather.Api.Services;

internal sealed class UserService(IOptions<JWTSettings> options) : IUserService
{
   private readonly JWTSettings _jwtSettings = options.Value;
   private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

   public string CreateJwtToken(User user, ICollection<Role> userRoles)
   {
      var roleClaims = new Collection<Claim>();
      foreach (var role in userRoles)
      {
         roleClaims.Add(new Claim("roles", role?.Name ?? string.Empty));
      }

      var claims = new[]
      {
         new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
         new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
      }
      .Union(roleClaims);

      var symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key));
      var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

      var jwtSecurityToken = new JwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
          signingCredentials: signingCredentials);

      return _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
   }
}
