using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace EasyTest.WebAPI.IntegrationTests.ControllersTests
{
	public class SessionControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;
		private readonly HttpClient _client;
		private readonly HttpClient _authorizedClient;

		public SessionControllerTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
			_client = _factory.CreateClient();
			_authorizedClient = _factory
				.AuthenticatedInstance()
				.CreateClient(new()
				{
					AllowAutoRedirect = false,
				});
		}

		[Fact]
		public async Task SessionController_StartSession_ReturnsOk()
		{
			// Arrange
			var sessionCreateDto = new SessionCreateDto()
			{
				TestId = Guid.NewGuid(),
				UserEmail = string.Empty
			};

			A.CallTo(() => _factory._sessionService.Create(A<SessionCreateDto>.Ignored))
				.Returns(new Response<SessionDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionDto(),
				});

			// Act
			var response = await _authorizedClient.PostAsync("/api/session", JsonContent.Create(sessionCreateDto));

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_StartSession_Returns401()
		{
			// Arrange
			var sessionCreateDto = A.Fake<SessionCreateDto>();

			// Act
			var response = await _client.PostAsync("/api/session", JsonContent.Create(sessionCreateDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

		}

		[Fact]
		public async Task SessionController_StartSession_ReturnsBadRequest()
		{
			// Arrange
			var sessionCreateDto = A.Fake<SessionCreateDto>();

			A.CallTo(() => _factory._sessionService.Create(A<SessionCreateDto>.Ignored))
				.Returns(new Response<SessionDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Invalid session data",
				});

			// Act
			var response = await _authorizedClient.PostAsync("/api/session", JsonContent.Create(sessionCreateDto));

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_ReturnsOk()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.IfGetResult(sessionId))
				.Returns(false);

			A.CallTo(() => _factory._sessionService.NextQuestion(sessionId))
				.Returns(new Response<QuestionNextDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new QuestionNextDto(),
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/session/{sessionId}/next");


			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_Returns401()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			// Act
			var response = await _client.GetAsync($"/api/session/{sessionId}/next");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_NotHaveHappened()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.IfGetResult(sessionId))
				.Returns(true);

			// Act
			var response = await _authorizedClient.GetAsync($"/api/session/{sessionId}/next");

			// Assert
			Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_ReturnsBadRequestOnError()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.IfGetResult(sessionId))
				.Returns(false);

			A.CallTo(() => _factory._sessionService.NextQuestion(sessionId))
				.Returns(new Response<QuestionNextDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting the next question",
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/session/{sessionId}/next");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_AnswerTheQuestion_ReturnsOk()
		{
			// Arrange
			var sessionId = Guid.NewGuid();
			var answerId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.AnswerTheQuestion(sessionId, answerId))
				.Returns(new Response<SessionAnswerDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionAnswerDto(),
				});

			// Act
			var response = await _authorizedClient.PostAsync($"/api/session/{sessionId}/answer/{answerId}", null);

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_AnswerTheQuestion_Returns401()
		{
			// Arrange
			var sessionId = Guid.NewGuid();
			var answerId = Guid.NewGuid();

			// Act
			var response = await _client.PostAsync($"/api/session/{sessionId}/answer/{answerId}", null);

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task SessionController_AnswerTheQuestion_ReturnsBadRequest()
		{
			// Arrange
			var sessionId = Guid.NewGuid();
			var answerId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.AnswerTheQuestion(sessionId, answerId))
				.Returns(new Response<SessionAnswerDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error answering the question",
				});

			// Act
			var response = await _authorizedClient.PostAsync($"/api/session/{sessionId}/answer/{answerId}", null);

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_GetResult_ReturnsOk()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.GetResult(sessionId))
				.Returns(new Response<SessionResultDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionResultDto(),
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/session/{sessionId}/result");

			// Assert
			Assert.True(response.IsSuccessStatusCode);
		}

		[Fact]
		public async Task SessionController_GetResult_Returns401()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			// Act
			var response = await _client.GetAsync($"/api/session/{sessionId}/result");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
			Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
		}

		[Fact]
		public async Task SessionController_GetResult_ReturnsBadRequest()
		{
			// Arrange
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _factory._sessionService.GetResult(sessionId))
				.Returns(new Response<SessionResultDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting the session result",
				});

			// Act
			var response = await _authorizedClient.GetAsync($"/api/session/{sessionId}/result");

			// Assert
			Assert.False(response.IsSuccessStatusCode);
		}
	}
}
