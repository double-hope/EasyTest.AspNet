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

        public async Task<Response<UserResponseDto>> Login(UserLoginDto userDto)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(user => user.Email.Equals(userDto.Email));
            if(user == null)
            {
                return Response<UserResponseDto>.Error("User does not found");
            }

            if(!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
				return Response<UserResponseDto>.Error("Provided password is wrong");
            }

            var userResponse = _mapper.Map<UserResponseDto>(user);
            return Response<UserResponseDto>.Success(userResponse, "You succesfully logged in");
        }

        public async Task<Response<UserResponseDto>> Register(UserRegisterDto userDto)
        {
            if (userDto.Role == UserRoles.Admin)
			{
				return Response<UserResponseDto>.Error("You cannot register user with role admin");
            }

            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(user => user.Email.Equals(userDto.Email));
            if (user != null)
			{
				return Response<UserResponseDto>.Error("User with this email already exists");
            }

            var userE = _mapper.Map<User>(userDto);
            var res = await _userManager.CreateAsync(userE, userDto.Password);

            if(!res.Succeeded)
			{
				return Response<UserResponseDto>.Error("Failed to register user", res.Errors.Select(e => e.Description).ToList());
            }

            res = await _userManager.AddToRoleAsync(userE, userDto.Role.ToString().ToLower());
            
            if (!res.Succeeded)
            {
				return Response<UserResponseDto>.Error("Failed to associate user with provided role", res.Errors.Select(e => e.Description).ToList());
            }

            var userResponse = _mapper.Map<UserResponseDto>(userE);
			return Response<UserResponseDto>.Success(userResponse, "You succesfully register user");
        }
    }
}
