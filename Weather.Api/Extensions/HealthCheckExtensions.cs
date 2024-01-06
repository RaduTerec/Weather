using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Weather.Api.Persistence;

namespace Weather.Api.Extensions;

internal static class HealthCheckExtensions
{
   internal static readonly string[] Tags = ["Database"];

   public static void AddApiHealthCheck(this WebApplicationBuilder builder)
   {
      builder.Services.AddHealthChecks()
                      .AddDbContextCheck<WeatherDbContext>("Database",
                                                            failureStatus: HealthStatus.Unhealthy,
                                                            tags: Tags);
   }

   public static void MapApiHealthCheck(this WebApplication app)
   {
      app.MapHealthChecks("/health", new HealthCheckOptions
      {
         ResponseWriter = WriteResponse,
      });
   }

   private static Task WriteResponse(HttpContext context, HealthReport healthReport)
   {
      context.Response.ContentType = "application/json; charset=utf-8";

      var options = new JsonWriterOptions { Indented = true };

      using var memoryStream = new MemoryStream();
      using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
      {
         jsonWriter.WriteStartObject();
         jsonWriter.WriteString("status", healthReport.Status.ToString());
         jsonWriter.WriteStartArray("results");

         foreach (var healthReportEntry in healthReport.Entries)
         {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("name", healthReportEntry.Key.ToString());
            jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
            jsonWriter.WriteEndObject();
         }

         jsonWriter.WriteEndArray();
         jsonWriter.WriteEndObject();
      }

      return context.Response.WriteAsync(
          Encoding.UTF8.GetString(memoryStream.ToArray()));
   }
}
