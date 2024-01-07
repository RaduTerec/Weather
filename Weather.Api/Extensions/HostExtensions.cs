using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Weather.Api.Core.Options;

namespace Weather.Api.Extensions;

internal static class HostExtensions
{
   public static void AddJwtAuthentication(this WebApplicationBuilder builder)
   {
      builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(nameof(JWTSettings)));
      var jwtSettings = builder.Configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>() ?? new JWTSettings();

      var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

      builder.Services.AddAuthentication(options =>
      {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            IssuerSigningKey = issuerSigningKey,
         };
      });
   }
}
