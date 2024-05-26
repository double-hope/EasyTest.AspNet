namespace EasyTest.Shared.DTO.Test
{
    public class UserTestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfAttempts { get; set; } = 1;
        public int TookedAttempts { get; set; } = 1;
        public int QuestionsAttempted { get; set; }
        public double BestResult { get; set; }
        public bool CanContinue { get; set; }
    }
}
