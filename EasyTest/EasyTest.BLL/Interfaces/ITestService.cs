using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.BLL.Interfaces
{
    public interface ITestService
    {
        Task<Response<IEnumerable<TestDto>>> GetAll();
        Task<Response<IEnumerable<UserTestDto>>> GetAll(string userEmail);
        Task<Response<TestDto>> Get(Guid id);
        Task<Response<UserTestDto>> Get(Guid id, string userEmail);
        Task<Response<TestDto>> Create(TestCreateDto testDto);
        Task<Response<TestDto>> Edit(Guid testId, TestEditDto testDto);
    }
}
