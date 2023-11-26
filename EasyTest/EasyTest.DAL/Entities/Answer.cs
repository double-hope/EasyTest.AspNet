using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
    public class Answer : BaseEntity<Guid>
    {
        public required Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
        [DisplayName("Answer Text")]
        public required string Text { get; set; }
        [DisplayName("Is Answer Correct")]
        public required bool IsCorrect { get; set; }
        public List<TestSession> Sessions { get; } = new();
        public List<SessionAnswer> SessionAnswers { get; } = new();
    }
}
