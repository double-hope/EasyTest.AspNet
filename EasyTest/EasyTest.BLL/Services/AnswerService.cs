using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Response;
using System.Collections.Generic;

namespace EasyTest.BLL.Services
{
	public class AnswerService : Service, IAnswerService
	{
		public AnswerService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

		public async Task<Response<IEnumerable<AnswerDto>>> CreateRange(List<AnswerDto> answersDtos, Guid questionId)
		{
			var answers = _mapper.Map<IEnumerable<Answer>>(answersDtos)
				.Select(answer =>
				{
					answer.QuestionId = questionId;
					return answer;
				});

			await _unitOfWork.AnswerRepository.AddRange(answers);
			return Response<IEnumerable<AnswerDto>>.Success(_mapper.Map<IEnumerable<AnswerDto>>(answers));
		}
	}
}
