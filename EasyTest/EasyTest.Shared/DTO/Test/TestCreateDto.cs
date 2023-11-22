namespace EasyTest.Shared.DTO.Test
{
    public class TestCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Guid> QuestionTestsIds { get; set; } = new();
    }
}
