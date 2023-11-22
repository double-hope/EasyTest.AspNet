using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.BLL.Services
{
    public class TestService : Service, ITestService
    {
        public TestService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<Response> CreateTest(TestCreateDto testDto)
        {
            var testE = _mapper.Map<Test>(testDto);
            var testQuestions = await _unitOfWork.QuestionRepository.GetAll(q => testDto.QuestionTestsIds.Any(x => x.Equals(q.Id)));
            testE.Title = testDto.Title;
            testE.Description = testDto.Description;
            testE.QuestionTests = testQuestions.Select(q => new QuestionTest { QuestionId = q.Id }).ToList();
            await _unitOfWork.TestRepository.Add(testE);
            await _unitOfWork.Save();

            return SuccessResponse(_mapper.Map<TestDto>(testE), "Test created successfully");
        }
    }
}
