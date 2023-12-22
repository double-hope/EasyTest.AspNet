using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.WebAPI.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Tests.Controllers
{
	[Trait("Category", "Unit")]
	public class QuestionControllerTests
	{
		private readonly IQuestionService _questionService;
		public QuestionControllerTests()
		{
			_questionService = A.Fake<IQuestionService>();
		}

		[Fact]
		public async Task QuestionController_CreateQuestions_ReturnsOk()
        {
            // Arrange
            var controller = new QuestionController(_questionService);
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			var sampleData = A.Fake<List<QuestionResponseDto>>();

			A.CallTo(() => _questionService.CreateMany(questionsDto, testId))
				.Returns(new Response<IEnumerable<QuestionResponseDto>>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = sampleData,
				});

            // Act
            var result = await controller.CreateQuestions(testId, questionsDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<QuestionResponseDto>>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
			Assert.Equal(sampleData, response.Data);
		}

		[Fact]
		public async Task QuestionController_CreateQuestions_ReturnsBadRequest()
        {
            // Arrange
            var controller = new QuestionController(_questionService);
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			A.CallTo(() => _questionService.CreateMany(questionsDto, testId))
				.Returns(new Response<IEnumerable<QuestionResponseDto>>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating questions",
				});

            // Act
            var result = await controller.CreateQuestions(testId, questionsDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<QuestionResponseDto>>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error creating questions", response.Message);
		}

		[Fact]
		public async Task QuestionController_EditQuestion_ReturnsOk()
		{
			// Arrange
			var controller = new QuestionController(_questionService);
			var questionId = Guid.NewGuid();
			var questionDto = A.Fake<QuestionDto>();

			var sampleData = A.Fake<QuestionResponseDto>();

			A.CallTo(() => _questionService.Edit(questionDto, questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = sampleData,
				});

			// Act
			var result = await controller.EditQuestion(questionId, questionDto);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<QuestionResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
			Assert.Equal(sampleData, response.Data);
		}

		[Fact]
		public async Task QuestionController_EditQuestion_ReturnsBadRequest()
		{
			// Arrange
			var controller = new QuestionController(_questionService);
			var questionId = Guid.NewGuid();
			var questionDto = A.Fake<QuestionDto>();

			A.CallTo(() => _questionService.Edit(questionDto, questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error editing question",
				});

			// Act
			var result = await controller.EditQuestion(questionId, questionDto);

			// Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<QuestionResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error editing question", response.Message);
		}


		[Fact]
		public async Task QuestionController_DeleteQuestion_ReturnsOk()
		{
			// Arrange
			var controller = new QuestionController(_questionService);
			var questionId = Guid.NewGuid();

			A.CallTo(() => _questionService.Delete(questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Success,
					Data = A.Fake<QuestionResponseDto>(),
				});

			// Act
			var result = await controller.DeleteQuestion(questionId);

			// Assert
			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<QuestionResponseDto>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
		}

		[Fact]
		public async Task QuestionController_DeleteQuestion_ReturnsBadRequest()
		{
			// Arrange
			var controller = new QuestionController(_questionService);
			var questionId = Guid.NewGuid();

			A.CallTo(() => _questionService.Delete(questionId))
				.Returns(new Response<QuestionResponseDto>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error deleting question",
				});

			// Act
			var result = await controller.DeleteQuestion(questionId);

			// Assert
			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<QuestionResponseDto>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error deleting question", response.Message);
		}
	}
}
