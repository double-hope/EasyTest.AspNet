using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Enums;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;
using Microsoft.AspNetCore.Identity;

namespace EasyTest.BLL.Services
{
    public class AuthService : Service, IAuthService
    {
        private readonly UserManager<User> _userManager;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        public async Task<Response> Login(UserLoginDto userDto)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(user => user.Email.Equals(userDto.Email));
            if(user == null)
            {
                return ErrorResponse("User does not found");
            }

            if(!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                return ErrorResponse("Provided password is wrong");
            }

            var userResponse = _mapper.Map<UserResponseDto>(user);
            return SuccessResponse(userResponse, "You succesfully logged in");
        }

        public async Task<Response> Register(UserRegisterDto userDto)
        {
            if (userDto.Role == UserRoles.Admin)
            {
                return ErrorResponse("You cannot register user with role admin");
            }

            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(user => user.Email.Equals(userDto.Email));
            if (user != null)
            {
                return ErrorResponse("User with this email already exists");
            }

            var userE = _mapper.Map<User>(userDto);
            var res = await _userManager.CreateAsync(userE, userDto.Password);

            if(!res.Succeeded)
            {
                return ErrorResponse("Failed to register user", res.Errors.Select(e => e.Description).ToList());
            }

            res = await _userManager.AddToRoleAsync(userE, userDto.Role.ToString().ToLower());
            
            if (!res.Succeeded)
            {
                return ErrorResponse("Failed to associate user with provided role", res.Errors.Select(e => e.Description).ToList());
            }

            var userResponse = _mapper.Map<UserResponseDto>(userE);
            return SuccessResponse(userResponse, "You succesfully register user");
        }
    }
}
