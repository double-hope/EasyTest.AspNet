using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class SessionAnswer<TKey> where TKey : IEquatable<TKey>
	{
		[Required]
		public int AnswerId { get; set; }
		[ForeignKey(nameof(AnswerId))]
		public Answer<TKey> Answer { get; set; }

		[Required]
		public int SessionId { get; set; }
		[ForeignKey(nameof(SessionId))]
		public TestSession<TKey> Session { get; set; }
		public bool IsCorrect{ get; set; }
	}
}
