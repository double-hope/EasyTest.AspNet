using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Tests.Controllers
{
	public class TestControllerTest
	{
		private readonly ITestService _testService;

		public TestControllerTest()
		{
			_testService = A.Fake<ITestService>();
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsOk()
		{
			var controller = new TestController(_testService);

			A.CallTo(() => _testService.GetAll())
				.Returns(new Response<IEnumerable<TestDto>>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new List<TestDto>(),
				});

			var result = await controller.GetTests();

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<TestDto>>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsBadRequest()
		{
			var controller = new TestController(_testService);

			A.CallTo(() => _testService.GetAll())
				.Returns(new Response<IEnumerable<TestDto>>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting tests",
				});

			var result = await controller.GetTests();

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<TestDto>>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting tests", response.Message);
		}

		[Fact]
		public async Task TestController_GetTest_ReturnsOk()
		{
			var controller = new TestController(_testService);
			var testId = Guid.NewGuid();

			A.CallTo(() => _testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new TestDto()
				});

			var result = await controller.GetTest(testId);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_GetTest_ReturnsBadRequest()
		{
			var controller = new TestController(_testService);
			var testId = Guid.NewGuid();

			A.CallTo(() => _testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting test",
				});

			var result = await controller.GetTest(testId);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting test", response.Message);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsOk()
		{
			var controller = new TestController(_testService);
			var testDto = A.Fake<TestCreateDto>();

			A.CallTo(() => _testService.Create(testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = A.Fake<TestDto>()
				});

			var result = await controller.CreateTest(testDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsBadRequest()
		{
			var controller = new TestController(_testService);
			var testDto = A.Fake<TestCreateDto>();

			A.CallTo(() => _testService.Create(testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating test",
				});

			var result = await controller.CreateTest(testDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error creating test", response.Message);
		}

		[Fact]
		public async Task TestController_EditTest_ReturnsOk()
		{
			var controller = new TestController(_testService);
			var testId = Guid.NewGuid();
			var testDto = A.Fake<TestEditDto>();

			A.CallTo(() => _testService.Edit(testId, testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = A.Fake<TestDto>()
				});

			var result = await controller.EditTest(testId, testDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task TestController_EditTest_ReturnsBadRequest()
		{
			var controller = new TestController(_testService);
			var testId = Guid.NewGuid();
			var testDto = A.Fake<TestEditDto>();

			A.CallTo(() => _testService.Edit(testId, testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error editing test",
				});

			var result = await controller.EditTest(testId, testDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<TestDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error editing test", response.Message);
		}
	}
}
