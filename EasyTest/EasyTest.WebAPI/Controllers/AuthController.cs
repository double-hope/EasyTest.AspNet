using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace EasyTest.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody]UserLoginDto userDto)
        {
            var response = await _authService.Login(userDto);

            if(response.Status == ResponseStatusCodes.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
