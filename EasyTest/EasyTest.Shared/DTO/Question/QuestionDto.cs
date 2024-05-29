using EasyTest.Shared.DTO.Answer;

namespace EasyTest.Shared.DTO.Question
{
	public class QuestionDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
		public List<AnswerDto> Answers { get; set; }
	}
}
