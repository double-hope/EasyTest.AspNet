using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Interfaces
{
	public interface IUserService
	{
		Task<UserDto> RegisterUser(UserRegisterDto userDto);
	}
}
