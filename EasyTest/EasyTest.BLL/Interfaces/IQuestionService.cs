using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;

namespace EasyTest.BLL.Interfaces
{
	public interface IQuestionService
	{
		Task<Response<QuestionResponseDto>> Create(QuestionDto questionDto, Guid testId);
	}
}
