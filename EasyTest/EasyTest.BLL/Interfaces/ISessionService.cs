using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;

namespace EasyTest.BLL.Interfaces
{
	public interface ISessionService
	{
		Task<Response<SessionDto>> Create(SessionCreateDto sessionDto);
		Task<Response<QuestionDto>> NextQuestion(Guid sessionId);
		Task<Response<SessionAnswerDto>> AnswerTheQuestion(Guid sessionId, Guid answerId);
	}
}
