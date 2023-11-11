namespace EasyTest.DAL.Entities
{
	public class QuestionTest
	{
		public Guid QuestionId { get; set; }
		public Guid TestId { get; set; }
		public Question Question { get; set; } = null!;
		public Test Test { get; set; } = null!;
	}
}
