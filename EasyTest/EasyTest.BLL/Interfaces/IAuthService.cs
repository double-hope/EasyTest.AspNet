using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<Response> Login(UserLoginDto userDto);
    }
}
