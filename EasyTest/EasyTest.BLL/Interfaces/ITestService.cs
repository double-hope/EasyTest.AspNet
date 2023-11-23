using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.BLL.Interfaces
{
    public interface ITestService
    {
        Task<Response> CreateTest(TestCreateDto testDto);
    }
}
