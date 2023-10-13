namespace EasyTest.DAL.Entities
{
	public class QuestionTest
	{
		public int QuestionId { get; set; }
		public int TestId { get; set; }
		public Question Question { get; set; } = null!;
		public Test Test { get; set; } = null!;
	}
}
