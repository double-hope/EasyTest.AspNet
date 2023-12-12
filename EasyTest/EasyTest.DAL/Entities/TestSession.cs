using EasyTest.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
    public class TestSession : BaseEntity<Guid>
    {
        public required Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
		public required Guid TestId { get; set; }
        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; }
        public TestStatus Status { get; set; }
        public List<Question> Questions { get; } = new();
        public List<SessionQuestion> SessionQuestions { get; } = new();
        public List<Answer> Answers { get; } = new();
        public List<SessionAnswer> SessionAnswers { get; } = new();
    }
}
