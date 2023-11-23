using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
    public class Question : BaseEntity<Guid>
    {
        [Required]
        [DisplayName("Question Title")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Question Text")]
        public string Text { get; set; }
        public List<Test> Tests { get; } = new();
        public List<QuestionTest> QuestionTests { get; } = new();
        public List<TestSession> TestSessions { get; } = new();
        public List<SessionQuestion> SessionQuestions { get; } = new();
    }
}
