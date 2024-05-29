using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Response;

namespace EasyTest.BLL.Interfaces
{
	public interface IAnswerService
	{
		Task<Response<IEnumerable<AnswerDto>>> CreateRange(List<AnswerDto> answersDtos, Guid questionId);
		Task<Response<IEnumerable<AnswerDto>>> EditRange(List<AnswerDto> answersDtos, Guid questionId);
	}
}
