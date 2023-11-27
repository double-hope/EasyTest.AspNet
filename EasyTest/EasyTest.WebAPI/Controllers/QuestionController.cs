using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/question")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
			_questionService = questionService;
		}

        [HttpPost("testId/{id}")]
        [ProducesResponseType(typeof(Response<QuestionResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateTest(Guid id, [FromBody] QuestionDto questionDto)
        {
            var response = await _questionService.Create(questionDto, id);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [HttpPost("testId/{id}")]
        [ProducesResponseType(typeof(Response<IEnumerable<QuestionResponseDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateTests(Guid id, [FromBody]IEnumerable<QuestionDto> questionsDto)
        {
            var response = await _questionService.CreateMany(questionsDto, id);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
