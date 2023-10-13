using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class SessionQuestion
	{
		[Required]
		public int QuestionId { get; set; }
		[ForeignKey(nameof(QuestionId))]
		public Question Question { get; set; }

		[Required]
		public int SessionId { get; set; }
		[ForeignKey(nameof(SessionId))]
		public TestSession Session { get; set; }
		public bool IsAnswered { get; set; }
	}
}
