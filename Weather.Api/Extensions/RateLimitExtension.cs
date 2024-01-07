using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using Weather.Api.Core;
using Weather.Api.Core.Options;

namespace Weather.Api.Extensions;

internal static class RateLimitExtension
{
   private const string ProblemDetailsContentType = "application/problem+json";

   private static readonly JsonSerializerOptions _jsonSerializationOptions = new()
   {
      DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
   };

   public static void AddApiRateLimits(this WebApplicationBuilder builder)
   {
      var rateLimitSettings = new RateLimitSettings();
      builder.Configuration.GetSection(nameof(RateLimitSettings)).Bind(rateLimitSettings);

      builder.Services.AddRateLimiter(limiterOptions =>
      {
         limiterOptions.OnRejected = async (context, _)
            => await SetRejectionResponseAsync(context, rateLimitSettings);

         // restrictions for the login page, to limit the number of passwords to try.
         limiterOptions.AddPolicy(Constants.LoginPolicy, context
            => SetLoginPolicy(context, rateLimitSettings));

         // restrictions for the register page, to how often someone can register
         limiterOptions.AddPolicy(Constants.RegisterPolicy, context
            => SetRegisterPolicy(context, rateLimitSettings));

         limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context
            => SetGlobalPolicy(context, rateLimitSettings));
      });
   }

   private static async Task SetRejectionResponseAsync(OnRejectedContext context, RateLimitSettings rateLimitSettings)
   {
      // could potentially replace this with a call to ProblemDetailsFactory
      // but since it's only this request, it makes no sense.
      var problemDetails = new ProblemDetails
      {
         Title = "Too Many Requests",
         Detail = $"Please try again in {rateLimitSettings.Window} second(s).",
         Status = StatusCodes.Status429TooManyRequests,
         Type = "https://www.rfc-editor.org/rfc/rfc6585#section-4",
      };

      context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
      context.HttpContext.Response.ContentType = ProblemDetailsContentType;

      var content = JsonSerializer.Serialize(problemDetails, _jsonSerializationOptions);

      await context.HttpContext.Response.WriteAsync(content, context.HttpContext.RequestAborted);
   }

   private static RateLimitPartition<IPAddress> SetLoginPolicy(HttpContext context, RateLimitSettings rateLimitSettings)
   {
      var remoteIpAddress = context.Connection.RemoteIpAddress;

      return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress!, _ => new FixedWindowRateLimiterOptions
      {
         PermitLimit = rateLimitSettings.LoginLimit,
         Window = TimeSpan.FromSeconds(rateLimitSettings.LoginWindow),
         QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
         QueueLimit = 0,
      });
   }

   private static RateLimitPartition<IPAddress> SetRegisterPolicy(HttpContext context, RateLimitSettings rateLimitSettings)
   {
      var remoteIpAddress = context.Connection.RemoteIpAddress;

      return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress!, _ => new FixedWindowRateLimiterOptions
      {
         PermitLimit = rateLimitSettings.RegisterLimit,
         Window = TimeSpan.FromSeconds(rateLimitSettings.Window),
         QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
         QueueLimit = 0,
      });
   }

   private static RateLimitPartition<IPAddress> SetGlobalPolicy(HttpContext context, RateLimitSettings rateLimitSettings)
   {
      var remoteIpAddress = context.Connection.RemoteIpAddress;

      if (remoteIpAddress != default && !IPAddress.IsLoopback(remoteIpAddress))
      {
         return RateLimitPartition.GetFixedWindowLimiter(remoteIpAddress!, _ => new FixedWindowRateLimiterOptions
         {
            PermitLimit = rateLimitSettings.GlobalLimit,
            Window = TimeSpan.FromSeconds(rateLimitSettings.Window),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0,
         });
      }

      return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
   }
}
