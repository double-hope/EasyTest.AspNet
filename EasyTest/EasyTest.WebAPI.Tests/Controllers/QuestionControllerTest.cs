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
	public class QuestionControllerTest
	{
		private readonly IQuestionService _questionService;
		public QuestionControllerTest()
		{
			_questionService = A.Fake<IQuestionService>();
		}

		[Fact]
		public async Task QuestionController_CreateTests_ReturnsOk()
		{
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

			var result = await controller.CreateTests(testId, questionsDto);

			var okObjectResult = Assert.IsType<OkObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<QuestionResponseDto>>>(okObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Success, response.Status);
			Assert.NotNull(response.Data);
			Assert.Equal(sampleData, response.Data);
		}

		[Fact]
		public async Task QuestionController_CreateTests_ReturnsBadRequest()
		{
			var controller = new QuestionController(_questionService);
			var testId = Guid.NewGuid();
			var questionsDto = A.Fake<List<QuestionDto>>();

			A.CallTo(() => _questionService.CreateMany(questionsDto, testId))
				.Returns(new Response<IEnumerable<QuestionResponseDto>>
				{
					Status = ResponseStatusCodesConst.Error,
					Message = "Error creating questions",
				});

			var result = await controller.CreateTests(testId, questionsDto);

			var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
			var response = Assert.IsType<Response<IEnumerable<QuestionResponseDto>>>(badRequestObjectResult.Value);

			Assert.Equal(ResponseStatusCodesConst.Error, response.Status);
			Assert.Equal("Error creating questions", response.Message);
		}
	}
}
