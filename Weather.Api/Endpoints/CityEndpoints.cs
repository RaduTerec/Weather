using Microsoft.AspNetCore.Mvc;

using Weather.Api.Core;
using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Models;

namespace Weather.Api.Endpoints;

public static class CityEndpoints
{
   public static RouteGroupBuilder ToCityEndpoints(this RouteGroupBuilder cityItems)
   {
      cityItems.MapGet("/{id:int}", GetAsync).WithTags("Public");
      cityItems.MapPost("/", InsertAsync).RequireAuthorization(Constants.AdminAuthPolicy).WithTags("Private");
      cityItems.MapPut("/{id:int}", UpdateAsync).RequireAuthorization(Constants.AdminAuthPolicy).WithTags("Private");
      cityItems.MapDelete("/{id:int}", DeleteAsync).RequireAuthorization(Constants.AdminAuthPolicy).WithTags("Private");

      return cityItems;
   }

   public static async Task<IResult> GetAsync([FromRoute] int id, IUnitOfWork unitOfWork)
   {
      var city = await unitOfWork.Cities.GetAsync(id);
      if (city == default)
      {
         return TypedResults.Problem(detail: "City not found", statusCode: StatusCodes.Status404NotFound);
      }

      var cityDto = new CityDTO { Id = city.Id, Name = city.Name };
      return TypedResults.Ok(cityDto);
   }

   public static async Task<IResult> InsertAsync([FromBody] CityDTO cityDto, IUnitOfWork unitOfWork)
   {
      if (cityDto.Id != default)
      {
         return TypedResults.Problem(detail: "Id cannot be set for a new city", statusCode: StatusCodes.Status400BadRequest);
      }

      bool alreadyExists = await unitOfWork.Cities.ExistsAsync(cityDto.Name);
      if (alreadyExists)
      {
         return TypedResults.Problem(detail: "This city already exists", statusCode: StatusCodes.Status400BadRequest);
      }

      var city = new City { Name = cityDto.Name };
      await unitOfWork.Cities.AddAsync(city);
      await unitOfWork.CommitAsync();

      var insertedCityDto = new CityDTO { Id = city.Id, Name = city.Name };
      return TypedResults.Ok(insertedCityDto);
   }

   public static async Task<IResult> UpdateAsync([FromRoute] int id, [FromBody] CityDTO cityDto, IUnitOfWork unitOfWork)
   {
      var city = await unitOfWork.Cities.GetAsync(id);
      if (city == default)
      {
         return TypedResults.Problem(detail: "City not found", statusCode: StatusCodes.Status404NotFound);
      }

      bool alreadyExists = await unitOfWork.Cities.ExistsAsync(cityDto.Name);
      if (alreadyExists)
      {
         return TypedResults.Problem(detail: "This city already exists", statusCode: StatusCodes.Status400BadRequest);
      }

      city.Name = cityDto.Name;
      await unitOfWork.CommitAsync();

      var updatedCityDto = new CityDTO { Id = city.Id, Name = city.Name };
      return TypedResults.Ok(updatedCityDto);
   }

   public static async Task<IResult> DeleteAsync([FromRoute] int id, IUnitOfWork unitOfWork)
   {
      var city = await unitOfWork.Cities.GetWithMeasurements(id);
      if (city == default)
      {
         return TypedResults.Problem(detail: "City not found", statusCode: StatusCodes.Status404NotFound);
      }
      if (city.Measurements != default && city.Measurements.Count != 0)
      {
         return TypedResults.Problem(detail: "Cannot delete a city with measurements", statusCode: StatusCodes.Status400BadRequest);
      }

      unitOfWork.Cities.Remove(city);
      await unitOfWork.CommitAsync();

      return TypedResults.Ok(id);
   }
}
