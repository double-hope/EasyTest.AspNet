using System.ComponentModel;

namespace EasyTest.DAL.Entities
{
    public class Test : BaseEntity<Guid>
    {
        [DisplayName("Test Name")]
        public required string Title { get; set; }
        [DisplayName("Test Decription")]
        public required string Description { get; set; }
		[DisplayName("Number Of Attempts")]
		public int NumberOfAttempts { get; set; } = 1;
		[DisplayName("Questions Attempted")]
		public int QuestionsAttempted { get; set; }
		public List<Question> Questions { get; } = new();
        public List<QuestionTest> QuestionTests { get; } = new();
    }
}
