using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Tests.Controllers
{
	public class TestControllerTests
	{
		private readonly ITestService _testService;

		public TestControllerTests()
		{
			_testService = A.Fake<ITestService>();
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsOk()
        {
            // Arrange
            var controller = new TestController(_testService);

			A.CallTo(() => _testService.GetAll())
				.Returns(new Response<IEnumerable<TestDto>>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new List<TestDto>(),
				});

            // Act
            var result = await controller.GetTests();

            // Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<TestDto>>>(okObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsBadRequest()
        {
            // Arrange
            var controller = new TestController(_testService);

			A.CallTo(() => _testService.GetAll())
				.Returns(new Response<IEnumerable<TestDto>>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting tests",
				});

            // Act
            var result = await controller.GetTests();

            // Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<TestDto>>>(badRequestObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting tests", response.Message);
		}

		[Fact]
		public async Task TestController_GetTest_ReturnsOk()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testId = Guid.NewGuid();

			A.CallTo(() => _testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new TestDto()
				});

            // Act
            var result = await controller.GetTest(testId);

            // Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_GetTest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testId = Guid.NewGuid();

			A.CallTo(() => _testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting test",
				});

            // Act
            var result = await controller.GetTest(testId);

            // Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting test", response.Message);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsOk()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testDto = A.Fake<TestCreateDto>();

			A.CallTo(() => _testService.Create(testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = A.Fake<TestDto>()
				});

            // Act
            var result = await controller.CreateTest(testDto);

            // Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testDto = A.Fake<TestCreateDto>();

			A.CallTo(() => _testService.Create(testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating test",
				});

            // Act
            var result = await controller.CreateTest(testDto);

            // Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error creating test", response.Message);
		}

		[Fact]
		public async Task TestController_EditTest_ReturnsOk()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testId = Guid.NewGuid();
			var testDto = A.Fake<TestEditDto>();

			A.CallTo(() => _testService.Edit(testId, testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = A.Fake<TestDto>()
				});

            // Act
            var result = await controller.EditTest(testId, testDto);

            // Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_EditTest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new TestController(_testService);
			var testId = Guid.NewGuid();
			var testDto = A.Fake<TestEditDto>();

			A.CallTo(() => _testService.Edit(testId, testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error editing test",
				});

            // Act
            var result = await controller.EditTest(testId, testDto);

            // Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

            Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error editing test", response.Message);
		}
	}
}
