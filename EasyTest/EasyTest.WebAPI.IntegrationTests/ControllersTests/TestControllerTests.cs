using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using FakeItEasy;
using Newtonsoft.Json;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class TestControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private CustomWebApplicationFactory _factory;
		private HttpClient _client;

		public TestControllerTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
			_client = _factory
				.AuthenticatedInstance()
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
			var response = await _client.GetAsync("/api/test");

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
		public async Task TestController_GetTests_ReturnsBadRequest()
		{
			// Act
			var response = await _client.GetAsync("/api/test");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}

		public void Dispose()
		{
			_client.Dispose();
			_factory.Dispose();
		}
	}
}
