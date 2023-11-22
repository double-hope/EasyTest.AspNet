using EasyTest.BLL.Interfaces;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<ActionResult> LoginUser([FromBody]UserLoginDto userDto)
        {
            var response = await _authService.Login(userDto);

            if(response.Status == Status.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
