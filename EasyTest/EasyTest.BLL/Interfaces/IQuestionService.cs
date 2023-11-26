using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;

namespace EasyTest.BLL.Interfaces
{
	public interface IQuestionService
	{
		Task<Response<QuestionDto>> Create(QuestionDto questionDto, Guid testId);
	}
}
