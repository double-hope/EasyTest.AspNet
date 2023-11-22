using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class Answer : BaseEntity<Guid>
	{
		[Required]
		public int QuestionId { get; set; }
		[ForeignKey(nameof(QuestionId))]
		public Question Question { get; set; }
		[Required]
		[DisplayName("Answer Text")]
		public string Text { get; set; }
		[Required]
		[DisplayName("Is Answer Correct")]
		public bool IsCorrect { get; set; }
		public List<SessionAnswer> SessionAnswers { get; set; } = new();
	}
}
