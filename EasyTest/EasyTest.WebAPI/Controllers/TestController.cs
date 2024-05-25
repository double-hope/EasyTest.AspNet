using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
		}

        [HttpGet]
		[ProducesResponseType(typeof(Response<IEnumerable<TestDto>>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> GetTests()
        {
            var response = await _testService.GetAll();

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(Response<IEnumerable<UserTestDto>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetUserTests()
        {
            if (HttpContext.Items.TryGetValue("UserEmail", out var userEmailObj) && userEmailObj is string userEmail)
            {
                var response = await _testService.GetAll(userEmail);

                if (response.Status == ResponseStatusCodesConst.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }

            return Unauthorized(Response<UserTestDto>.Error("Token not found"));
        }

        [HttpGet("{id}")]
		[ProducesResponseType(typeof(Response<TestDto>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult> GetTest(Guid id)
        {
            var response = await _testService.Get(id);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [HttpPost]
		[ProducesResponseType(typeof(Response<TestDto>), (int)HttpStatusCode.OK)]
		[Authorize(Roles = $"{UserRolesConst.AdminRole},{UserRolesConst.TeacherRole}")]
		public async Task<ActionResult> CreateTest([FromBody]TestCreateDto testDto)
        {
            var response = await _testService.Create(testDto);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(Response<TestDto>), (int)HttpStatusCode.OK)]
		[Authorize(Roles = $"{UserRolesConst.AdminRole},{UserRolesConst.TeacherRole}")]
		public async Task<ActionResult> EditTest(Guid id, [FromBody] TestEditDto testDto)
		{
			var response = await _testService.Edit(id, testDto);

			if (response.Status == ResponseStatusCodesConst.Success)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}
