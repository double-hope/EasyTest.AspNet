using System.Net;

using Microsoft.AspNetCore.Mvc;

using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using EasyTest.Shared.DTO.Session;

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
        [ProducesResponseType(typeof(Response<UserResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> LoginUser([FromBody] UserLoginDto userDto)
        {
            var response = await _authService.Login(userDto);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                var refreshToken = await _authService.GenerateToken(response.Data.AccessToken);
                Response.Cookies.Append("X-Refresh-Token", refreshToken.Data, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Response<UserResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto userDto)
        {
            var response = await _authService.Register(userDto);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                var refreshToken = await _authService.GenerateToken(response.Data.AccessToken);
                Response.Cookies.Append("X-Refresh-Token", refreshToken.Data, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["X-Refresh-Token"];
            var response = await _authService.GenerateToken(refreshToken);

            if (response.Status == ResponseStatusCodesConst.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var cookies = Request.Cookies.Keys;
            foreach (var cookie in cookies)
            {
                Response.Cookies.Delete(cookie);
            }

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("user")]
        public async Task<ActionResult> GetUser()
        {
            if (HttpContext.Items.TryGetValue("UserEmail", out var userEmailObj) && userEmailObj is string userEmail)
            {
                var response = await _authService.GetUser(userEmail);

                if (response.Status == ResponseStatusCodesConst.Success)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }

            return Unauthorized(Response<SessionDto>.Error("Token not found"));
        }
    }
}
