using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.BLL.Interfaces
{
    public interface ITestService
    {
        Task<Response<IEnumerable<TestDto>>> GetAll();
        Task<Response<TestDto>> Get(Guid id);
        Task<Response<TestDto>> Create(TestCreateDto testDto);
    }
}
