using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class TestSession<TKey> where TKey : IEquatable<TKey>
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public User<TKey> User { get; set; }

		[Required]
		public int TestId { get; set; }
		[ForeignKey(nameof(TestId))]
		public Test<TKey> Test { get; set; }
		public string Status { get; set; }
		public List<SessionQuestion<TKey>> SessionQuestions { get; set; } = new();
		public List<SessionAnswer<TKey>> SessionAnswers { get; set; } = new();
	}
}
