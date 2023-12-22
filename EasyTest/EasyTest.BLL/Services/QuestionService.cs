using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;

namespace EasyTest.BLL.Services
{
	public class QuestionService : Service, IQuestionService
	{
		private readonly IAnswerService _answerService;
		public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IAnswerService answerService) : base(unitOfWork, mapper)
		{
			_answerService = answerService;
		}
		public async Task<Response<IEnumerable<QuestionResponseDto>>> CreateMany(IEnumerable<QuestionDto>questionsDto, Guid testId)
		{
            try
            {
                using var transaction = await _unitOfWork.BeginTransaction();

                var responses = new List<QuestionResponseDto>();

                foreach (var q in questionsDto)
                {
                    var response = await Create(q, testId);
                    responses.Add(response.Data);

                    if (response.Status == ResponseStatusCodesConst.Error)
                    {
                        await _unitOfWork.Rollback();
						return Response<IEnumerable<QuestionResponseDto>>.Error(response.Message);
                    }
                }

                await _unitOfWork.Commit().ConfigureAwait(false);

                return Response<IEnumerable<QuestionResponseDto>>.Success(responses);
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
				List<string>? errors = new List<string> { ex.Message };

                return Response<IEnumerable<QuestionResponseDto>>.Error("An error occurred", errors);

            }
        }
		public async Task<Response<QuestionResponseDto>> Create(QuestionDto questionDto, Guid testId)
		{
			var questionE = _mapper.Map<Question>(questionDto);

			await _unitOfWork.QuestionRepository.Add(questionE);

			var testE = await _unitOfWork.TestRepository.GetById(testId);

			if (testE == null)
			{
				return Response<QuestionResponseDto>.Error("Test not found");
			}

			var questionTest = new QuestionTest
			{
				QuestionId = questionE.Id,
				TestId = testE.Id
			};

			await _unitOfWork.QuestionTestRepository.Add(questionTest);

			var res = await _answerService.CreateRange(questionDto.Answers, questionE.Id);

			if (res.Status == ResponseStatusCodesConst.Error)
			{
				return Response<QuestionResponseDto>.Error("Fail adding question");
			}

			await _unitOfWork.Save();
			return Response<QuestionResponseDto>.Success(_mapper.Map<QuestionResponseDto>(questionE));
		}

		public async Task<Response<QuestionResponseDto>> Edit(QuestionDto questionDto, Guid questionId)
		{
			var question = await _unitOfWork.QuestionRepository.GetById(questionId);

			if (question == null) return Response<QuestionResponseDto>.Error("Question does not found");

			_mapper.Map(questionDto, question);

			question.UpdatedAt = DateTime.UtcNow;

			_unitOfWork.QuestionRepository.Update(question);
			await _unitOfWork.Save();

			return Response<QuestionResponseDto>.Success(_mapper.Map<QuestionResponseDto>(question), "Question updated successfully");
		}

		public async Task<Response<QuestionResponseDto>> Delete(Guid questionId)
		{
			var question = await _unitOfWork.QuestionRepository.GetById(questionId);

			if (question == null) return Response<QuestionResponseDto>.Error("Question does not found");

			var answers = await _unitOfWork.AnswerRepository.GetByQuestionId(questionId);
			foreach (var answer in answers)
			{
				_unitOfWork.AnswerRepository.Remove(answer);
			}

			_unitOfWork.QuestionRepository.Remove(question);

			await _unitOfWork.Save();

			return Response<QuestionResponseDto>.Success(_mapper.Map<QuestionResponseDto>(question), "Question and associated answers deleted successfully");

		}
	}
}
