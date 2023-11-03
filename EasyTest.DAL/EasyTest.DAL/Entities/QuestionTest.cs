namespace EasyTest.DAL.Entities
{
	public class QuestionTest<TKey> where TKey : IEquatable<TKey>
	{
		public int QuestionId { get; set; }
		public int TestId { get; set; }
		public Question<TKey> Question { get; set; } = null!;
		public Test<TKey> Test { get; set; } = null!;
	}
}
