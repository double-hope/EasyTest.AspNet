using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
    public class SessionAnswer
    {
        public required Guid AnswerId { get; set; }
        [ForeignKey(nameof(AnswerId))]
        public Answer Answer { get; set; }

        public required Guid SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public TestSession Session { get; set; }
        public bool IsCorrect { get; set; }
    }
}
