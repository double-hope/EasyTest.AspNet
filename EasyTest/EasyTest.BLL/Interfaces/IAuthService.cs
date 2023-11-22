using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> Login(UserLoginDto userDto);
    }
}
