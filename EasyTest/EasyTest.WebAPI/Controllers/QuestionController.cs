﻿using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/question")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
			_questionService = questionService;
		}

        //[HttpPost("testId/{id}")]
        //[ProducesResponseType(typeof(Response<QuestionResponseDto>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> CreateTest(Guid id, [FromBody] QuestionDto questionDto)
        //{
        //    var response = await _questionService.Create(questionDto, id);

        //    if (response.Status == ResponseStatusCodesConst.Success)
        //    {
        //        return Ok(response);
        //    }

        //    return BadRequest(response);
        //}
        [HttpPost("testId/{id}")]
        [ProducesResponseType(typeof(Response<IEnumerable<QuestionResponseDto>>), (int)HttpStatusCode.OK)]
		[Authorize(Roles = $"{UserRolesConst.AdminRole},{UserRolesConst.TeacherRole}")]
		public async Task<ActionResult> CreateQuestions(Guid id, [FromBody]IEnumerable<QuestionDto> questionsDto)
        {
            var response = await _questionService.CreateMany(questionsDto, id);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Response<QuestionResponseDto>), (int)HttpStatusCode.OK)]
		[Authorize(Roles = $"{UserRolesConst.AdminRole},{UserRolesConst.TeacherRole}")]
		public async Task<ActionResult> EditQuestion(Guid id, [FromBody] QuestionDto questionDto)
		{
			var response = await _questionService.Edit(questionDto, id);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(Response<QuestionResponseDto>), (int)HttpStatusCode.OK)]
		[Authorize(Roles = $"{UserRolesConst.AdminRole},{UserRolesConst.TeacherRole}")]
		public async Task<ActionResult> DeleteQuestion(Guid id)
		{
			var response = await _questionService.Delete(id);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}
