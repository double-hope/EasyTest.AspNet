using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.Helpers;
using FakeItEasy;
using Newtonsoft.Json;
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

		[Fact]
		public async Task QuestionController_EditTest_ReturnsOk()
		{
			// Arrange
			var questionId = Guid.NewGuid();
			var questionDto = new QuestionDto()
			{
				Title = "Title#1",
				Text = "Test to check you...",
				Answers = new List<AnswerDto>()
				{
					new AnswerDto()
					{
						Text = "First",
						IsCorrect = true
					}
				}
			};
			var sampleData = A.Fake<QuestionResponseDto>();

			A.CallTo(() => _factory._questionService.Edit(A<QuestionDto>._, questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = sampleData,
				});

			// Act
			var response = await _privilegedClient.PutAsync($"/api/question/{questionId}", JsonContent.Create(questionDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task QuestionController_EditTest_Returns403()
		{
			// Arrange
			var questionId = Guid.NewGuid();
			var questionDto = A.Fake<QuestionDto>();

			// Act
			var response = await _authorizedClient.PutAsync($"/api/question/{questionId}", JsonContent.Create(questionDto));

			// Assert
			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
		}

		[Fact]
		public async Task QuestionController_EditTest_Returns401()
		{
			// Arrange
			var questionId = Guid.NewGuid();
			var questionDto = A.Fake<QuestionDto>();

			// Act
			var response = await _client.PutAsync($"/api/question/{questionId}", JsonContent.Create(questionDto));

			// Assert
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task QuestionController_EditTest_ReturnsBadRequest()
		{
			// Arrange
			var questionId = Guid.NewGuid();
			var questionDto = A.Fake<QuestionDto>();

			A.CallTo(() => _factory._questionService.Edit(questionDto, questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error editing question",
				});

			// Act
			var response = await _privilegedClient.PutAsync($"/api/question/{questionId}", JsonContent.Create(questionDto));

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
