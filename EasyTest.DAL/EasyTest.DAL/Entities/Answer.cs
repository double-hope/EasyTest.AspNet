using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class Answer<TKey> where TKey : IEquatable<TKey>
	{
		[Key] 
		public TKey Id { get; set; }
		[Required]
		public int QuestionId { get; set; }
		[ForeignKey(nameof(QuestionId))]
		public Question<TKey> Question { get; set; }
		[Required]
		[DisplayName("Answer Text")]
		public string Text { get; set; }
		[Required]
		[DisplayName("Is Answer Correct")]
		public bool IsCorrect { get; set; }
		public List<SessionAnswer<TKey>> SessionAnswers { get; set; } = new();
	}
}
