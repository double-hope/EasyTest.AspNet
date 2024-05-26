using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

using EasyTest.Shared.Helpers;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Enums;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Services
{
    public class AuthService : Service, IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly AuthOptions _authOptions;
		public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IOptions<AuthOptions> authOptions) : base(unitOfWork, mapper)
		{
			_userManager = userManager;
			_authOptions = authOptions.Value;
		}

        public async Task<Response<UserResponseDto>> Login(UserLoginDto userDto)
		{
			var user = await _unitOfWork.UserRepository.GetByEmail(userDto.Email);
			if (user == null)
			{
				return Response<UserResponseDto>.Error("User does not found");
			}

			if (!await _userManager.CheckPasswordAsync(user, userDto.Password))
			{
				return Response<UserResponseDto>.Error("Provided password is wrong");
			}

			var userResponse = new UserResponseDto
			{
				AccessToken = await GenerateJwt(user)
			};

			return Response<UserResponseDto>.Success(userResponse, "You successfully logged in");
		}

		public async Task<Response<UserResponseDto>> Register(UserRegisterDto userDto)
		{
			if (userDto.Role == UserRoles.Admin)
			{
				return Response<UserResponseDto>.Error("You cannot register user with role admin");
			}

			var user = await _unitOfWork.UserRepository.GetByEmail(userDto.Email);
			if (user != null)
			{
				return Response<UserResponseDto>.Error("User with this email already exists");
			}

			var userE = _mapper.Map<User>(userDto);
			var res = await _userManager.CreateAsync(userE, userDto.Password);

			if (!res.Succeeded)
			{
				return Response<UserResponseDto>.Error("Failed to register user", res.Errors.Select(e => e.Description).ToList());
			}

			res = await _userManager.AddToRoleAsync(userE, userDto.Role.ToString().ToLower());

			if (!res.Succeeded)
			{
				return Response<UserResponseDto>.Error("Failed to associate user with provided role", res.Errors.Select(e => e.Description).ToList());
			}

			var userResponse = new UserResponseDto
			{
				AccessToken = await GenerateJwt(userE)
			};

			return Response<UserResponseDto>.Success(userResponse, "You successfully register user");
        }

        public async Task<Response<UserDto>> GetUser(string userEmail)
        {
            var dbUser = await _unitOfWork.UserRepository.GetByEmail(userEmail);
            if (dbUser != null)
            {
                return Response<UserDto>.Error("User with this email does not exist");
            }

            var user = _mapper.Map<UserDto>(dbUser);

            return Response<UserDto>.Success(user, "Returning user");
        }

        public async Task<Response<string>> GenerateToken(string inToken)
        {
            var email = DecodeJwt(inToken);

            if (string.IsNullOrEmpty(email))
            {
                return Response<string>.Error("Wrong refresh token");
            }

            var user = await _unitOfWork.UserRepository.GetByEmail(email);
            if (user == null)
            {
                return Response<string>.Error($"User with email {email} was not found");
            }

			return Response<string>.Success(await GenerateJwt(user));
        }

        #region helpers
        private async Task<string> GenerateJwt(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role", string.Join(",", userRoles))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityTokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _authOptions.Issuer,
                Audience = _authOptions.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_authOptions.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Key)), SecurityAlgorithms.HmacSha256)
            };

            var jwt = tokenHandler.CreateToken(securityTokenDescription);
            return tokenHandler.WriteToken(jwt);
        }

        private string DecodeJwt(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.ReadToken(jwtToken) as JwtSecurityToken;

            if (token != null)
            {
                return token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email || claim.Type == "email")?.Value;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
