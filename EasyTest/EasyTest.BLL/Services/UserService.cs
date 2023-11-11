using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL;
using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Services
{
	public class UserService : Service, IUserService
	{
		public UserService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
		{
			
		}

		public Task<UserDto> RegisterUser(UserRegisterDto userDto)
		{
			throw new NotImplementedException();
		}
	}
}
