using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
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

        public async Task<UserDto> Login(UserLoginDto userDto)
        {
            var user = _unitOfWork.UserRepository.GetFirstOrDefault(user => user.Email.Equals(userDto.Email));
            if(user == null)
            {
                throw new Exception("Not Found");
            }

            if(!await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                throw new Exception("Wrong Password");
            }

            //return
            throw new NotImplementedException();
        }
    }
}
