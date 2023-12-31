﻿using EasyTest.BLL.Interfaces;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
		public async Task<ActionResult> LoginUser([FromBody]UserLoginDto userDto)
        {
            var response = await _authService.Login(userDto);

            if(response.Status == ResponseStatusCodesConst.Success)
            {
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
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
