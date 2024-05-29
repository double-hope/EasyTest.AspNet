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
        public async Task<Response<IEnumerable<TestDto>>> GetAll()
        {
            var res = await _unitOfWork.TestRepository.GetAll();
			return Response<IEnumerable<TestDto>>.Success(_mapper.Map<IEnumerable<TestDto>>(res));
        }

        public async Task<Response<IEnumerable<UserTestDto>>> GetAll(string userEmail)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(userEmail);

            if (user == null)
            {
                return Response<IEnumerable<UserTestDto>>.Error("User does not found");
            }

            var dbTests = await _unitOfWork.TestRepository.GetAll();
            var tests = _mapper.Map<IEnumerable<UserTestDto>>(dbTests);

            foreach (var test in tests)
            {
                var userTest = await _unitOfWork.UserTestRepository.GetByUserIdAndTestId(user.Id, test.Id);

                test.TookedAttempts = userTest?.NumberOfAttempts ?? 0;
                test.BestResult = userTest?.BestResult ?? 0;
                test.CanContinue = userTest?.CanContinue ?? false;
            }

            return Response<IEnumerable<UserTestDto>>.Success(tests);
        }

        public async Task<Response<TestDto>> Get(Guid id)
        {
            var res = await _unitOfWork.TestRepository.GetById(id);

            if(res == null) return Response<TestDto>.Error("Test does not found");

			return Response<TestDto>.Success(_mapper.Map<TestDto>(res));
        }

        public async Task<Response<UserTestDto>> Get(Guid id, string userEmail)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(userEmail);

            if (user == null)
            {
                return Response<UserTestDto>.Error("User does not found");
            }

            var dbTest = await _unitOfWork.TestRepository.GetById(id);

            if (dbTest == null) return Response<UserTestDto>.Error("Test does not found");

            var test = _mapper.Map<UserTestDto>(dbTest);
            
            var userTest = await _unitOfWork.UserTestRepository.GetByUserIdAndTestId(user.Id, test.Id);

            test.TookedAttempts = userTest?.NumberOfAttempts ?? 0;
            test.BestResult = userTest?.BestResult ?? 0;
            test.CanContinue = userTest?.CanContinue ?? false;

            return Response<UserTestDto>.Success(test);
        }

        public async Task<Response<TestDto>> Create(TestCreateDto testDto)
        {
            var testE = _mapper.Map<Test>(testDto);
            
            await _unitOfWork.TestRepository.Add(testE);
            await _unitOfWork.Save();

			return Response<TestDto>.Success(_mapper.Map<TestDto>(testE), "Test created successfully");
        }
		public async Task<Response<TestDto>> Edit(Guid testId, TestEditDto testDto)
		{
            var test = await _unitOfWork.TestRepository.GetById(testId);

            if(test == null) return Response<TestDto>.Error("Test does not found");

			_mapper.Map(testDto, test);

			_unitOfWork.TestRepository.Update(test);
			await _unitOfWork.Save();

			return Response<TestDto>.Success(_mapper.Map<TestDto>(test), "Test updated successfully");
		}
    }
}
