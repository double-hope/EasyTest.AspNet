using System.ComponentModel;

namespace EasyTest.DAL.Entities
{
    public class Question : BaseEntity<Guid>
    {
        [DisplayName("Question Title")]
        public required string Title { get; set; }
        [DisplayName("Question Text")]
        public required string Text { get; set; }
        public List<Test> Tests { get; } = new();
        public List<QuestionTest> QuestionTests { get; } = new();
        public List<TestSession> TestSessions { get; } = new();
        public List<SessionQuestion> SessionQuestions { get; } = new();
    }
}
