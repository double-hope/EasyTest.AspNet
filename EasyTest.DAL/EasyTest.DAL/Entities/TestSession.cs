using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class TestSession
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public Guid UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }

		[Required]
		public Guid TestId { get; set; }
		[ForeignKey(nameof(TestId))]
		public Test Test { get; set; }
		public string Status { get; set; }
		public List<SessionQuestion> SessionQuestions { get; set; } = new();
		public List<SessionAnswer> SessionAnswers { get; set; } = new();
	}
}
