using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyTest.WebAPI.Controllers
{
	[ApiController]
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
			var response = await _sessionService.Create(sessionDto);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpGet("{id}/next")]
		[ProducesResponseType(typeof(Response<SessionDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> GetNextQuestion(Guid id)
		{
			var response = await _sessionService.NextQuestion(id);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}
