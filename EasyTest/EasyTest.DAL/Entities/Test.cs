using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
    public class Test : BaseEntity<Guid>
    {
        [Required]
        [DisplayName("Test Name")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Test Decription")]
        public string Description { get; set; }
        public List<Question> Questions { get; } = new();
        public List<QuestionTest> QuestionTests { get; } = new();
    }
}
