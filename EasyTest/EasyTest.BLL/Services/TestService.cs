using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;
using static System.Net.Mime.MediaTypeNames;

namespace EasyTest.BLL.Services
{
    public class TestService : Service, ITestService
    {
        public TestService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
        public async Task<Response> GetAll()
        {
            var res = await _unitOfWork.TestRepository.GetAll();
            return SuccessResponse(_mapper.Map<IEnumerable<TestDto>>(res));
        }
        public async Task<Response> Get(Guid id)
        {
            var res = await _unitOfWork.TestRepository.GetFirstOrDefault(x => x.Id == id);
            return SuccessResponse(_mapper.Map<TestDto>(res));
        }
        public async Task<Response> Create(TestCreateDto testDto)
        {
            var testE = _mapper.Map<Test>(testDto);
            
            await _unitOfWork.TestRepository.Add(testE);
            await _unitOfWork.Save();

            return SuccessResponse(_mapper.Map<TestDto>(testE), "Test created successfully");
        }
    }
}
