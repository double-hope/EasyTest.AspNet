using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
    public class SessionQuestion
    {
        public required Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }

        public required Guid SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public TestSession Session { get; set; }
        public bool IsAnswered { get; set; }
    }
}
