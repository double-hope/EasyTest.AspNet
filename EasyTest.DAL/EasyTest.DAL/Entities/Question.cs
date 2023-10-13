using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class Question
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[DisplayName("Question Title")]
		public string Title { get; set; }
		[Required]
		[DisplayName("Question Text")]
		public string Text { get; set; }
		public List<QuestionTest> QuestionTests { get; set; } = new();
		public List<SessionQuestion> SessionQuestions { get; set; } = new();
	}
}
