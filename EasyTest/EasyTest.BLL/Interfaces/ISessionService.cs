using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;

namespace EasyTest.BLL.Interfaces
{
	public interface ISessionService
	{
		Task<Response<SessionDto>> Create(SessionCreateDto sessionDto, string userEmail);
		Task<Response<QuestionNextDto>> NextQuestion(Guid sessionId);
		Task<Response<SessionAnswerDto>> AnswerTheQuestion(Guid sessionId, Guid answerId);
		Task<bool> IfGetResult(Guid sessionId);
		Task<Response<SessionResultDto>> GetResult(Guid sessionId);
	}
}
