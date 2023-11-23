using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
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

            return SuccessResponse(user, "You succesfully logged in");
        }

        public async Task<Response> Register(UserRegisterDto userDto)
        {
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

            return SuccessResponse(user, "You succesfully logged in");
        }
    }
}
