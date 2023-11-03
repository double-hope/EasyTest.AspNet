using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class Test<TKey> where TKey : IEquatable<TKey>
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[DisplayName("Test Name")]
		public string Title { get; set; }
		[Required]
		[DisplayName("Test Decription")]
		public string Description { get; set; }
		public List<QuestionTest<TKey>> QuestionTests { get; set; } = new();
	}
}
