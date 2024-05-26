using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class UserTest : BaseEntity<Guid>
	{
		public required Guid TestId { get; set; }
		[ForeignKey(nameof(TestId))]
		public Test Test { get; set; }
		public required Guid UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public User User { get; set; }
		public required int NumberOfAttempts { get; set; } = 0;
		public double BestResult { get; set; } = 0;
		public bool CanContinue;
    }
}
