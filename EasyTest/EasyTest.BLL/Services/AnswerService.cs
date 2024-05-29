using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;

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

        public async Task<Response<IEnumerable<AnswerDto>>> EditRange(List<AnswerDto> answersDtos, Guid questionId)
        {
            var allDbAnswers = await _unitOfWork.AnswerRepository.GetByQuestionId(questionId);
            var answerIds = new HashSet<Guid>(answersDtos.Select(a => a.Id));

            foreach (var dbAnswer in allDbAnswers)
            {
                if (!answerIds.Contains(dbAnswer.Id))
                {
                    _unitOfWork.AnswerRepository.Remove(dbAnswer);
                }
            }

            foreach (var answer in answersDtos)
            {
                var dbAnswer = await _unitOfWork.AnswerRepository.GetById(answer.Id);

                if (dbAnswer == null)
                {
                    var newDbAnswer = _mapper.Map<Answer>(answer);
                    newDbAnswer.QuestionId = questionId;

                    await _unitOfWork.AnswerRepository.Add(newDbAnswer);
                }
                else
                {
                    _mapper.Map(answer, dbAnswer);

                    _unitOfWork.AnswerRepository.Update(dbAnswer);
                }
            }

            return Response<IEnumerable<AnswerDto>>.Success(answersDtos);
        }
    }
}