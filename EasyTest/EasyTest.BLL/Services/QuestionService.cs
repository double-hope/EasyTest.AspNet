﻿using AutoMapper;
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
            catch (Exception)
            {
                throw;
            }
        }
		public async Task<Response<QuestionResponseDto>> Create(QuestionDto questionDto, Guid testId)
		{
			var questionE = _mapper.Map<Question>(questionDto);

			await _unitOfWork.QuestionRepository.Add(questionE);

			var testE = await _unitOfWork.TestRepository.GetFirstOrDefault(x => x.Id == testId);

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
    }
}