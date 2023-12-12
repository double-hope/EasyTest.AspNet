namespace EasyTest.Shared.DTO.Test
{
    public class TestEditDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfAttempts { get; set; } = 1;
        public int QuestionsAttempted { get; set; }
	}
}
