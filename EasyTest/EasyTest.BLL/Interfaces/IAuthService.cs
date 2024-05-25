using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<Response<UserResponseDto>> Login(UserLoginDto userDto);
        Task<Response<UserResponseDto>> Register(UserRegisterDto userDto);
	    Task<Response<string>> GenerateToken(string inToken);
    }
}
