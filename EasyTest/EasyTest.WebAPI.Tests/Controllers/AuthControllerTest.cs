using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using EasyTest.Shared.Enums;

namespace EasyTest.WebAPI.Tests.Controllers
{
	public class AuthControllerTest
	{
		private readonly IAuthService _authService;
		public AuthControllerTest()
		{
			_authService = A.Fake<IAuthService>();
		}

		[Fact]
		public async Task AuthController_LoginUser_ReturnsOk()
		{
			var controller = new AuthController(_authService);
			var userDto = new UserLoginDto
			{
				Email = "email@gmail.com",
				Password = "12#$qwER"
			};

			A.CallTo(() => _authService.Login(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new UserResponseDto(),
				});

			var result = await controller.LoginUser(userDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task AuthController_LoginUser_ReturnsBadRequest()
		{
			var controller = new AuthController(_authService);
			var userDto = new UserLoginDto
			{
				Email = "email@gmail.com",
				Password = "12#$qwER"
			};

			A.CallTo(() => _authService.Login(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error logging in",
				});

			var result = await controller.LoginUser(userDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error logging in", response.Message);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsOk()
		{
			var controller = new AuthController(_authService);
			var userDto = new UserRegisterDto 
			{
				Name = "Test",
				Email = "email@gmail.com",
				Password = "12#$qwER",
				Role = UserRoles.Student
			};

			A.CallTo(() => _authService.Register(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new UserResponseDto()
				});

			var result = await controller.RegisterUser(userDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsBadRequest()
		{
			var controller = new AuthController(_authService);
			var userDto = new UserRegisterDto
			{
				Name = "Test",
				Email = "email@gmail.com",
				Password = "12#$qwER",
				Role = UserRoles.Student
			};

			A.CallTo(() => _authService.Register(userDto))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error registering user",
				});

			var result = await controller.RegisterUser(userDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<UserResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error registering user", response.Message);
		}
	}
}
