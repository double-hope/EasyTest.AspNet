using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
    public class QuestionTest
    {
        [Required]
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; } = null!;

        [Required]
        public Guid TestId { get; set; }
        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; } = null!;
    }
}
