using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Tests.Controllers
{
	[Trait("Category", "Unit")]
	public class AuthControllerTests
	{
		private readonly IAuthService _authService;
		public AuthControllerTests()
		{
			_authService = A.Fake<IAuthService>();
		}

		[Fact]
		public async Task AuthController_LoginUser_ReturnsOk()
		{
			// Arrange
			var userDto = A.Fake<UserLoginDto>();
			var userResponseDto = A.Fake<UserResponseDto>();

			var authService = A.Fake<IAuthService>();

			A.CallTo(() => authService.Login(userDto))
				.Returns(Task.FromResult(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = userResponseDto,
				}));

			A.CallTo(() => authService.GenerateToken(A<string>._))
				.Returns(Task.FromResult(new Response<string>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = "fake-refresh-token",
				}));

			var controller = new AuthController(authService);

			var httpContext = new DefaultHttpContext();
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

			// Act
			var result = await controller.LoginUser(userDto);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);

			Assert.True(httpContext.Response.Headers.ContainsKey("Set-Cookie"));
			var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
			Assert.Contains("X-Refresh-Token=fake-refresh-token", setCookieHeader);
		}

		[Fact]
		public async Task AuthController_LoginUser_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authService);
			var userDto = A.Fake<UserLoginDto>();

			A.CallTo(() => _authService.Login(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error logging in",
				});

            // Act
            var result = await controller.LoginUser(userDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error logging in", response.Message);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsOk()
		{
			// Arrange
			var userDto = A.Fake<UserRegisterDto>();
			var userResponseDto = A.Fake<UserResponseDto>();

			var authService = A.Fake<IAuthService>();

			A.CallTo(() => authService.Register(userDto))
				.Returns(Task.FromResult(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = userResponseDto
				}));

			A.CallTo(() => authService.GenerateToken(A<string>._))
				.Returns(Task.FromResult(new Response<string>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = "fake-refresh-token",
				}));

			var controller = new AuthController(authService);

			var httpContext = new DefaultHttpContext();
			controller.ControllerContext = new ControllerContext
			{
				HttpContext = httpContext
			};

			// Act
			var result = await controller.RegisterUser(userDto);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);

			Assert.True(httpContext.Response.Headers.ContainsKey("Set-Cookie"));
			var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
			Assert.Contains("X-Refresh-Token=fake-refresh-token", setCookieHeader);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsBadRequest()
        {
            // Arrange
            var controller = new AuthController(_authService);
			var userDto = A.Fake<UserRegisterDto>();

			A.CallTo(() => _authService.Register(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error registering user",
				});

            // Act
            var result = await controller.RegisterUser(userDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error registering user", response.Message);
		}
	}
}
