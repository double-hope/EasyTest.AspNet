using EasyTest.Shared.DTO.Answer;

namespace EasyTest.Shared.DTO.Question
{
	public class QuestionNextDto
	{
		public string Title { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
		public List<AnswerNextDto> Answers { get; set; }
	}
}
