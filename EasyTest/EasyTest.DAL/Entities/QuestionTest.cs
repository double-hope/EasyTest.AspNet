using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
    public class QuestionTest
    {
        public required Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;

        public required Guid TestId { get; set; }
        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; } = null!;
    }
}
