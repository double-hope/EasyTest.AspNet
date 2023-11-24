using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Test;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        [HttpGet]
        public async Task<ActionResult> GetTests()
        {
            var response = await _testService.GetAll();

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
        [HttpGet("{id}")]
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
        public async Task<ActionResult> CreateTest([FromBody]TestCreateDto testDto)
        {
            var response = await _testService.Create(testDto);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
