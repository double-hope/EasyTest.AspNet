using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Tests.Controllers
{
	public class SessionControllerTest
	{
		private readonly ISessionService _sessionService;
		public SessionControllerTest()
		{
			_sessionService = A.Fake<ISessionService>();
		}

		[Fact]
		public async Task SessionController_StartSession_ReturnsOk()
		{
			var controller = new SessionController(_sessionService);
			var sessionCreateDto = A.Fake<SessionCreateDto>();

			A.CallTo(() => _sessionService.Create(A<SessionCreateDto>.Ignored))
				.Returns(new Response<SessionDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionDto(),
				});

			var result = await controller.StartSession(sessionCreateDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<SessionDto>>(okObjectResult.Value);
			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task SessionController_StartSession_ReturnsBadRequest()
		{
			var controller = new SessionController(_sessionService);
			var sessionCreateDto = A.Fake<SessionCreateDto>();

			A.CallTo(() => _sessionService.Create(A<SessionCreateDto>.Ignored))
				.Returns(new Response<SessionDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Invalid session data",
				});

			var result = await controller.StartSession(sessionCreateDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<SessionDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Invalid session data", response.Message);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_ReturnsOk()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _sessionService.IfGetResult(sessionId))
				.Returns(false);

			A.CallTo(() => _sessionService.NextQuestion(sessionId))
				.Returns(new Response<QuestionNextDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new QuestionNextDto(),
				});

			var result = await controller.GetNextQuestion(sessionId);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<QuestionNextDto>>(okObjectResult.Value);
			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_NotHaveHappened()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _sessionService.IfGetResult(sessionId))
				.Returns(true);

			var result = await controller.GetNextQuestion(sessionId);

			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal(nameof(SessionController.GetResult), redirectToActionResult.ActionName);

			A.CallTo(() => _sessionService.NextQuestion(A<Guid>.Ignored)).MustNotHaveHappened();
		}

		[Fact]
		public async Task SessionController_GetNextQuestion_ReturnsBadRequestOnError()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _sessionService.IfGetResult(sessionId))
				.Returns(false);

			A.CallTo(() => _sessionService.NextQuestion(sessionId))
				.Returns(new Response<QuestionNextDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting the next question",
				});

			var result = await controller.GetNextQuestion(sessionId);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<QuestionNextDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting the next question", response.Message);
		}

		[Fact]
		public async Task SessionController_AnswerTheQuestion_ReturnsOk()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();
			var answerId = Guid.NewGuid();

			A.CallTo(() => _sessionService.AnswerTheQuestion(sessionId, answerId))
				.Returns(new Response<SessionAnswerDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionAnswerDto(),
				});

			var result = await controller.AnswerTheQuestion(sessionId, answerId);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<SessionAnswerDto>>(okObjectResult.Value);
			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task SessionController_AnswerTheQuestion_ReturnsBadRequest()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();
			var answerId = Guid.NewGuid();

			A.CallTo(() => _sessionService.AnswerTheQuestion(sessionId, answerId))
				.Returns(new Response<SessionAnswerDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error answering the question",
				});

			var result = await controller.AnswerTheQuestion(sessionId, answerId);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<SessionAnswerDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error answering the question", response.Message);
		}

		[Fact]
		public async Task SessionController_GetResult_ReturnsOk()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _sessionService.GetResult(sessionId))
				.Returns(new Response<SessionResultDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = new SessionResultDto(),
				});

			var result = await controller.GetResult(sessionId);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<SessionResultDto>>(okObjectResult.Value);
			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task SessionController_GetResult_ReturnsBadRequest()
		{
			var controller = new SessionController(_sessionService);
			var sessionId = Guid.NewGuid();

			A.CallTo(() => _sessionService.GetResult(sessionId))
				.Returns(new Response<SessionResultDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error getting the session result",
				});

			var result = await controller.GetResult(sessionId);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<SessionResultDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error getting the session result", response.Message);
		}
	}
}
