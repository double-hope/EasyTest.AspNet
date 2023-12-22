using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using FakeItEasy;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	[Trait("Category", "Integration")]
	public class QuestionControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;
		private readonly HttpClient _client;
		private readonly HttpClient _authorizedClient;
		private readonly HttpClient _privilegedClient;
		public QuestionControllerTests(CustomWebApplicationFactory factory)
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
		public async Task QuestionController_CreateTests_ReturnsOk()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();
			var sampleData = A.Fake<List<QuestionResponseDto>>();

			A.CallTo(() => _factory._questionService.CreateMany(A<IEnumerable<QuestionDto>>._, A<Guid>._))
				.Returns(new Response<IEnumerable<QuestionResponseDto>>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = sampleData,
				});

			// Act
			var response = await _privilegedClient.PostAsync($"/api/question/testId/{testId}", JsonContent.Create(questionsDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task QuestionController_CreateTests_Returns403()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			// Act
			var response = await _authorizedClient.PostAsync($"/api/question/testId/{testId}", JsonContent.Create(questionsDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
		}

		[Fact]
		public async Task QuestionController_CreateTests_Returns401()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			// Act
			var response = await _client.PostAsync($"/api/question/testId/{testId}", JsonContent.Create(questionsDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task QuestionController_CreateTests_ReturnsBadRequest()
		{
			// Arrange
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			A.CallTo(() => _factory._questionService.CreateMany(A<IEnumerable<QuestionDto>>._, A<Guid>._))
				.Returns(new Response<IEnumerable<QuestionResponseDto>>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating questions",
				});

			// Act
			var response = await _privilegedClient.PostAsync($"/api/question/testId/{testId}", JsonContent.Create(questionsDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

	}
}
