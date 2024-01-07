using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Weather.Unit.Tests.Common;

internal static class TypedResultExtensions
{
   public static T? GetObjectResult<T>(this IResult result)
   {
      if (result is not Ok<T>)
      {
         return default;
      }

      return (result as Ok<T>)!.Value;
   }

   public static ProblemDetails? GetProblemDetails(this IResult result)
   {
      if (result is not ProblemHttpResult)
      {
         return default;
      }

      return (result as ProblemHttpResult)!.ProblemDetails;
   }
}
