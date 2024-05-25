using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.WebAPI.Controllers
{
	[ApiController]
	[Authorize]
	[Route("/api/session")]
	public class SessionController : ControllerBase
	{
		private readonly ISessionService _sessionService;

		public SessionController(ISessionService sessionService)
		{
			_sessionService = sessionService;
		}

		[HttpPost]
		[ProducesResponseType(typeof(Response<SessionDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> StartSession([FromBody] SessionCreateDto sessionDto)
		{
            if (HttpContext.Items.TryGetValue("UserEmail", out var userEmailObj) && userEmailObj is string userEmail)
            {

                var response = await _sessionService.Create(sessionDto, userEmail);

                if (response.Status == ResponseStatusCodesConst.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }

            return Unauthorized(Response<UserTestDto>.Error("Token not found"));
		}

		[HttpGet("{id}/next")]
		[ProducesResponseType(typeof(Response<QuestionNextDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> GetNextQuestion(Guid id)
		{
			if (await _sessionService.IfGetResult(id))
			{
				return RedirectToAction(nameof(GetResult), new { id });
			}

			var response = await _sessionService.NextQuestion(id);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPost("{id}/answer/{answerId}")]
		[ProducesResponseType(typeof(Response<SessionAnswerDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> AnswerTheQuestion(Guid id, Guid answerId)
		{
			var response = await _sessionService.AnswerTheQuestion(id, answerId);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
		[HttpGet("{id}/result")]
		[ProducesResponseType(typeof(Response<SessionResultDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> GetResult(Guid id)
		{
			var response = await _sessionService.GetResult(id);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}
