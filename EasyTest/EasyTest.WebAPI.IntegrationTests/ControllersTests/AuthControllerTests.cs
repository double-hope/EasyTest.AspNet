using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.User;
using EasyTest.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class AuthControllerTests : IDisposable
	{
		private CustomWebApplicationFactory _factory;
		private HttpClient _client;

		public AuthControllerTests()
		{
			_factory = new CustomWebApplicationFactory();
			_client = _factory.CreateClient();
		}
		[Fact]
		public async Task AuthController_LoginUser_ReturnsOk()
		{
			// Arrange
			//var controller = new AuthController(_authService);
			//var userDto = A.Fake<UserLoginDto>();

			//A.CallTo(() => _authService.Login(userDto))
			//	.Returns(new Response<UserResponseDto>
			//	{
			//		Status = ResponseStatusCodesConst.Success,
			//		Data = new UserResponseDto(),
			//	});

			//var response = await _client.PostAsync("/api/auth/login");

			// Act
			//var result = await controller.LoginUser(userDto);

			// Assert
			//var okObjectResult = Assert.IsType<OkObjectResult>(result);
			//var response = Assert.IsType<Response<UserResponseDto>>(okObjectResult.Value);

			//Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			//Assert.NotNull(response.Data);
		}
		public void Dispose()
		{
			_client.Dispose();
			_factory.Dispose();
		}
	}
}
