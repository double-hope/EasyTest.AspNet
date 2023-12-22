using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.User;
using FakeItEasy;
using System.Net.Http.Json;
using EasyTest.Shared.DTO.Response;
using System.Net;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;
		private readonly HttpClient _client;

		public AuthControllerTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
			_client = _factory.CreateClient();
		}
		[Fact]
		public async Task AuthController_LoginUser_ReturnsOk()
		{
			// Arrange
			var userDto = new UserLoginDto()
			{
				Email = "nadia.prohorchuk@gmail.com",
				Password = "qwQW!@12"
			};

			A.CallTo(() => _factory._authService.Login(A<UserLoginDto>._))
			.Returns(new Response<UserResponseDto>
			{
				Status = ResponseStatusCodesConst.Success,
				Data = new UserResponseDto()
				{
					AccessToken = "fake_token"
				}
			});

			// Act
			var response = await _client.PostAsync("/api/auth/login", JsonContent.Create(userDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}


		[Fact]
		public async Task AuthController_LoginUser_ReturnsBadRequest()
		{
			// Arrange
			var userDto = A.Fake<UserLoginDto>();

			A.CallTo(() => _factory._authService.Login(A<UserLoginDto>._))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error logging in",
				});

			// Act
			var response = await _client.PostAsync("/api/auth/login", JsonContent.Create(userDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsOk()
		{
			// Arrange
			var userDto = A.Fake<UserRegisterDto>();

			A.CallTo(() => _factory._authService.Register(A<UserRegisterDto>._))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new UserResponseDto()
				});

			// Act
			var response = await _client.PostAsync("/api/auth/register", JsonContent.Create(userDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task AuthController_RegisterUser_ReturnsBadRequest()
		{
			// Arrange
			var userDto = A.Fake<UserRegisterDto>();

			A.CallTo(() => _factory._authService.Register(A<UserRegisterDto>._))
				.Returns(new Response<UserResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error registering user",
				});

			// Act
			var response = await _client.PostAsync("/api/auth/register", JsonContent.Create(userDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
