using Weather.Api.Core;
using Weather.Api.Core.DataTransferObjects;
using Weather.Api.Core.Models;
using Weather.Api.Core.Repositories;
using Weather.Api.Endpoints;
using Weather.Unit.Tests.Common;
using Weather.Unit.Tests.Common.Fakes;

namespace Weather.Unit.Tests.Endpoints;

public sealed class CityEndpointsTests
{
   private readonly ICityRepository _cityRepository;
   private readonly IUnitOfWork _unitOfWork;

   public CityEndpointsTests()
   {
      _cityRepository = new FakeCityRepository();
      _unitOfWork = new FakeUnitOfWork(cityRepository: _cityRepository);
   }

   [Fact]
   public async Task GetAsync_ReturnsNotFound_WhenNoCity()
   {
      const int NotFoundCityId = 2;

      // Act
      var result = await CityEndpoints.GetAsync(NotFoundCityId, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(404, problemDetail!.Status);
      Assert.Equal("City not found", problemDetail.Detail);
   }

   [Fact]
   public async Task InsertAsync_ReturnsBadRequest_WhenCityHasIdLargerThanZero()
   {
      var cityAttempt = new CityDTO { Id = 1, Name = "Basel" };

      // Act
      var result = await CityEndpoints.InsertAsync(cityAttempt, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("Id cannot be set for a new city", problemDetail.Detail);
   }

   [Fact]
   public async Task InsertAsync_ReturnsBadRequest_WhenCityAlreadyExists()
   {
      var cityAttempt = new CityDTO { Name = "Basel" };
      await _cityRepository.AddAsync(new City { Name = cityAttempt.Name });

      // Act
      var result = await CityEndpoints.InsertAsync(cityAttempt, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("This city already exists", problemDetail.Detail);
   }

   [Fact]
   public async Task UpdateAsync_ReturnsNotFound_WhenCityDoesNotExist()
   {
      var cityAttempt = new CityDTO { Id = 1, Name = "Basel" };

      // Act
      var result = await CityEndpoints.UpdateAsync(cityAttempt.Id, cityAttempt, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(404, problemDetail!.Status);
      Assert.Equal("City not found", problemDetail.Detail);
   }

   [Fact]
   public async Task UpdateAsync_ReturnsBadRequest_WhenCityAlreadyExists()
   {
      var cityAttempt = new CityDTO { Id = 2, Name = "Basel" };
      await _cityRepository.AddAsync(new City { Id = cityAttempt.Id, Name = cityAttempt.Name });

      // Act
      var result = await CityEndpoints.UpdateAsync(cityAttempt.Id, cityAttempt, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("This city already exists", problemDetail.Detail);
   }

   [Fact]
   public async Task DeleteAsync_ReturnsNotFound_WhenCityDoesNotExist()
   {
      const int NotFoundCityId = 3;

      // Act
      var result = await CityEndpoints.DeleteAsync(NotFoundCityId, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(404, problemDetail!.Status);
      Assert.Equal("City not found", problemDetail.Detail);
   }

   [Fact]
   public async Task DeleteAsync_ReturnsBadRequest_WhenCityHasMeasurements()
   {
      const int CityId = 3;
      await _cityRepository.AddAsync(new City { Id = CityId, Name = "Cluj", Measurements = [new()] });

      // Act
      var result = await CityEndpoints.DeleteAsync(CityId, _unitOfWork);
      var problemDetail = result.GetProblemDetails();

      Assert.Equal(400, problemDetail!.Status);
      Assert.Equal("Cannot delete a city with measurements", problemDetail.Detail);
   }
}
