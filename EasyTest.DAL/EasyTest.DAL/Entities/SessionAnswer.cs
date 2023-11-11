using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class SessionAnswer
	{
		[Required]
		public Guid AnswerId { get; set; }
		[ForeignKey(nameof(AnswerId))]
		public Answer Answer { get; set; }

		[Required]
		public Guid SessionId { get; set; }
		[ForeignKey(nameof(SessionId))]
		public TestSession Session { get; set; }
		public bool IsCorrect{ get; set; }
	}
}
