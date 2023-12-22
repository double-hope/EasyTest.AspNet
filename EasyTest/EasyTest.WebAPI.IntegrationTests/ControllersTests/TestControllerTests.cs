using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using FakeItEasy;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class TestControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;
		private readonly HttpClient _client;
		private readonly HttpClient _authorizedClient;
		private readonly HttpClient _privilegedClient;

		public TestControllerTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
			_client = _factory.CreateClient();
			_authorizedClient = _factory
				.AuthenticatedInstance()
				.CreateClient(new()
				{
					AllowAutoRedirect = false,
				});
			_privilegedClient = _factory
				.AuthenticatedInstance(
					new Claim(ClaimTypes.Role, UserRolesConst.AdminRole),
					new Claim(ClaimTypes.Role, UserRolesConst.TeacherRole)
				)
				.CreateClient(new()
				{
					AllowAutoRedirect = false,
				});
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsOk()
		{
			// Arrange

			var mockTests = new TestDto[]
			{
				new(){ Title = "Title#1", Description = "Description", NumberOfAttempts = 1 },
				new(){ Title = "Title#2", Description = "Description", NumberOfAttempts = 2 },
				new(){ Title = "Title#3", Description = "Description", NumberOfAttempts = 1 },
			}.AsEnumerable();

			A.CallTo(() => _factory._testService.GetAll())
				.Returns(new Response<IEnumerable<TestDto>>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = mockTests,
				});

			// Act
			var response = await _authorizedClient.GetAsync("/api/test");

			// Assert
			Assert.True(response.IsSuccessStatusCode);

			var content = await response.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<Response<IEnumerable<TestDto>>>(content);

			Assert.Collection(data.Data!,
				r =>
				{
					Assert.Equal("Title#1", r.Title);
					Assert.Equal("Description", r.Description);
					Assert.Equal(1, r.NumberOfAttempts);
				},
				r =>
				{
					Assert.Equal("Title#2", r.Title);
					Assert.Equal("Description", r.Description);
					Assert.Equal(2, r.NumberOfAttempts);
				},
				r =>
				{
					Assert.Equal("Title#3", r.Title);
					Assert.Equal("Description", r.Description);
					Assert.Equal(1, r.NumberOfAttempts);
				});
		}

		[Fact]
		public async Task TestController_GetTests_Returns401()
		{
			// Arrange

			// Act
			var response = await _client.GetAsync("/api/test");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task TestController_GetTests_ReturnsBadRequest()
		{
			// Act
			var response = await _client.GetAsync("/api/test");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}


		[Fact]
		public async Task TestController_GetTest_ReturnsOk()
		{
			// Arrange
			var testId = Guid.NewGuid();

			A.CallTo(() => _factory._testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new TestDto()
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/test/{testId}");

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task TestController_GetTest_Returns401()
		{
			// Arrange
			var testId = Guid.NewGuid();

			// Act
			var response = await _client.GetAsync($"/api/test/{testId}");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task TestController_GetTest_ReturnsBadRequest()
		{
			// Arrange
			var testId = Guid.NewGuid();

			A.CallTo(() => _factory._testService.Get(testId))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting test",
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/test/{testId}");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsOk()
		{
			// Arrange
			var testDto = new TestCreateDto() { Title = "Title#1", Description = "Description" };

			A.CallTo(() => _factory._testService.Create(A<TestCreateDto>._))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new TestDto()
					{
						Title = testDto.Title,
						Description = testDto.Description
					}
				});

			// Act

			var response = await _privilegedClient.PostAsync($"/api/test", JsonContent.Create(testDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);

			var content = await response.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<Response<TestDto>>(content);

			Assert.Equal("Title#1", data.Data.Title);
			Assert.Equal("Description", data.Data.Description);
			Assert.Equal(1, data.Data.NumberOfAttempts);
		}

		[Fact]
		public async Task TestController_CreateTest_ReturnsBadRequest()
		{
			// Arrange
			var testDto = A.Fake<TestCreateDto>();

			A.CallTo(() => _factory._testService.Create(testDto))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating test",
				});

			// Act
			string jsonString = JsonConvert.SerializeObject(testDto);
			var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

			var response = await _client.PostAsync($"/api/test/", content);

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task TestController_EditTest_ReturnsOk()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var testDto = new TestEditDto() { Title = "Title#1", Description = "Description", NumberOfAttempts = 3 };

			A.CallTo(() => _factory._testService.Edit(A<Guid>._, A<TestEditDto>._))
				.Returns(new Response<TestDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new TestDto()
					{
						Title = testDto.Title,
						Description = testDto.Description,
						NumberOfAttempts = testDto.NumberOfAttempts
					}
				});

			// Act

			var response = await _privilegedClient.PutAsync($"/api/test/{testId}", JsonContent.Create(testDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);

			var content = await response.Content.ReadAsStringAsync();

			var data = JsonConvert.DeserializeObject<Response<TestDto>>(content);

			Assert.Equal("Title#1", data.Data.Title);
			Assert.Equal("Description", data.Data.Description);
			Assert.Equal(3, data.Data.NumberOfAttempts);
		}

		[Fact]
		public async Task TestController_EditTest_Returns401()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var testDto = A.Fake<TestEditDto>();

			// Act

			var response = await _client.PutAsync($"/api/test/{testId}", JsonContent.Create(testDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
		}
	}
}
