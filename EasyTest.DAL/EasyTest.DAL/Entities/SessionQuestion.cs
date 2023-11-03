using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class SessionQuestion<TKey> where TKey : IEquatable<TKey>
	{
		[Required]
		public int QuestionId { get; set; }
		[ForeignKey(nameof(QuestionId))]
		public Question<TKey> Question { get; set; }

		[Required]
		public int SessionId { get; set; }
		[ForeignKey(nameof(SessionId))]
		public TestSession<TKey> Session { get; set; }
		public bool IsAnswered { get; set; }
	}
}
